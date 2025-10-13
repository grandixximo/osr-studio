// Changelog loader and display script
(function() {
    'use strict';

    const GITHUB_RAW_BASE = 'https://raw.githubusercontent.com/grandixximo/osr-studio';
    const CHANGELOG_PATHS = {
        'main': `${GITHUB_RAW_BASE}/main/docs/Changelogs/main.json`,
        'classic-ui': `${GITHUB_RAW_BASE}/classic-ui/docs/Changelogs/classic-ui.json`
    };

    const SECTION_ICONS = {
        'added': '✨',
        'changed': '🔄',
        'fixed': '🐛',
        'improved': '⚡',
        'removed': '🗑️',
        'security': '🔒',
        'deprecated': '⚠️'
    };

    let changelogData = {};
    let currentBranch = 'unified';

    // Initialize
    document.addEventListener('DOMContentLoaded', function() {
        setupTabs();
        loadChangelogs();
    });

    function setupTabs() {
        const tabs = document.querySelectorAll('.branch-tab');
        tabs.forEach(tab => {
            tab.addEventListener('click', function() {
                const branch = this.dataset.branch;
                switchBranch(branch);
            });
        });
    }

    function switchBranch(branch) {
        currentBranch = branch;
        
        // Update active tab
        document.querySelectorAll('.branch-tab').forEach(tab => {
            tab.classList.toggle('active', tab.dataset.branch === branch);
        });

        // Render the appropriate content
        renderChangelog();
    }

    async function loadChangelogs() {
        try {
            const [mainData, classicData] = await Promise.all([
                fetchChangelog('main'),
                fetchChangelog('classic-ui')
            ]);

            changelogData = {
                'main': mainData,
                'classic-ui': classicData
            };

            renderChangelog();
        } catch (error) {
            console.error('Error loading changelogs:', error);
            showError('Failed to load changelog data. Please try again later.');
        }
    }

    async function fetchChangelog(branch) {
        const response = await fetch(CHANGELOG_PATHS[branch]);
        if (!response.ok) {
            throw new Error(`Failed to fetch ${branch} changelog`);
        }
        return await response.json();
    }

    function renderChangelog() {
        const container = document.getElementById('changelog-content');
        
        if (!changelogData.main || !changelogData['classic-ui']) {
            container.innerHTML = '<div class="loading">Loading changelog...</div>';
            return;
        }

        let html = '';

        if (currentBranch === 'unified') {
            html = renderUnifiedView();
        } else {
            html = renderBranchView(currentBranch);
        }

        container.innerHTML = html;
    }

    function renderUnifiedView() {
        const mainReleases = changelogData.main.releases || [];
        const classicReleases = changelogData['classic-ui'].releases || [];

        // Merge releases by version
        const mergedReleases = mergeReleases(mainReleases, classicReleases);

        if (mergedReleases.length === 0) {
            return '<div class="error">No changelog entries found.</div>';
        }

        let html = '';
        mergedReleases.forEach(release => {
            html += renderUnifiedRelease(release);
        });

        return html;
    }

    function mergeReleases(mainReleases, classicReleases) {
        const releaseMap = new Map();

        // Add main branch releases
        mainReleases.forEach(release => {
            releaseMap.set(release.version, {
                version: release.version,
                date: release.date,
                title: release.title,
                main: release,
                classic: null
            });
        });

        // Add or merge classic branch releases
        classicReleases.forEach(release => {
            if (releaseMap.has(release.version)) {
                releaseMap.get(release.version).classic = release;
            } else {
                releaseMap.set(release.version, {
                    version: release.version,
                    date: release.date,
                    title: release.title,
                    main: null,
                    classic: release
                });
            }
        });

        // Convert to array and sort by version (descending)
        return Array.from(releaseMap.values()).sort((a, b) => {
            return compareVersions(b.version, a.version);
        });
    }

    function compareVersions(v1, v2) {
        const parts1 = v1.split('.').map(Number);
        const parts2 = v2.split('.').map(Number);
        
        for (let i = 0; i < Math.max(parts1.length, parts2.length); i++) {
            const part1 = parts1[i] || 0;
            const part2 = parts2[i] || 0;
            if (part1 !== part2) {
                return part1 - part2;
            }
        }
        return 0;
    }

    function renderUnifiedRelease(release) {
        let html = `
            <div class="release">
                <div class="release-header">
                    <div class="release-version">v${release.version}</div>
                    <div class="release-date">${formatDate(release.date)}</div>
                </div>
                <div class="release-title">${escapeHtml(release.title)}</div>
        `;

        if (release.main && release.classic) {
            // Both branches have this release
            html += '<h3 style="color: var(--primary-color); margin-top: 20px;">Modern UI (Main Branch)</h3>';
            html += renderSections(release.main.sections);
            html += '<h3 style="color: var(--primary-color); margin-top: 30px;">Classic UI</h3>';
            html += renderSections(release.classic.sections);
        } else if (release.main) {
            html += '<h3 style="color: var(--primary-color); margin-top: 20px;">Modern UI Only</h3>';
            html += renderSections(release.main.sections);
        } else if (release.classic) {
            html += '<h3 style="color: var(--primary-color); margin-top: 20px;">Classic UI Only</h3>';
            html += renderSections(release.classic.sections);
        }

        html += '</div>';
        return html;
    }

    function renderBranchView(branch) {
        const data = changelogData[branch];
        if (!data || !data.releases || data.releases.length === 0) {
            return '<div class="error">No changelog entries found for this branch.</div>';
        }

        let html = `
            <div style="text-align: center; margin-bottom: 30px;">
                <h2 style="color: var(--primary-color);">${escapeHtml(data.title)}</h2>
                <p style="color: var(--text-secondary);">${escapeHtml(data.description)}</p>
            </div>
        `;

        data.releases.forEach(release => {
            html += `
                <div class="release">
                    <div class="release-header">
                        <div class="release-version">v${release.version}</div>
                        <div class="release-date">${formatDate(release.date)}</div>
                    </div>
                    <div class="release-title">${escapeHtml(release.title)}</div>
                    ${renderSections(release.sections)}
                </div>
            `;
        });

        return html;
    }

    function renderSections(sections) {
        let html = '';
        sections.forEach(section => {
            const icon = SECTION_ICONS[section.type] || '📝';
            html += `
                <div class="section">
                    <div class="section-header">
                        <div class="section-icon ${section.type}">${icon}</div>
                        <div class="section-title">${escapeHtml(section.title)}</div>
                    </div>
                    <ul class="section-items">
                        ${section.items.map(item => `<li>${escapeHtml(item)}</li>`).join('')}
                    </ul>
                </div>
            `;
        });
        return html;
    }

    function formatDate(dateString) {
        if (!dateString || dateString.includes('XX')) {
            return 'Coming Soon';
        }
        try {
            const date = new Date(dateString);
            return date.toLocaleDateString('en-US', { 
                year: 'numeric', 
                month: 'long', 
                day: 'numeric' 
            });
        } catch (e) {
            return dateString;
        }
    }

    function escapeHtml(text) {
        const div = document.createElement('div');
        div.textContent = text;
        return div.innerHTML;
    }

    function showError(message) {
        const container = document.getElementById('changelog-content');
        container.innerHTML = `<div class="error">${escapeHtml(message)}</div>`;
    }
})();

