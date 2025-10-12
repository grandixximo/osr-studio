# Project Structure

## Base
`OsrStudio.Base` contains common interfaces and base classes. This project is referenced by all other projects.

## Localization
`OsrStudio.Loc` contains the localization code.

## Audio
`OsrStudio.Audio` contains the audio interfaces which are implemented by specific libraries like `OsrStudio.NAudio`.

## Screna, Core
`Screna` and `OsrStudio.Core` projects contain the bulk of the code and are depended on by both UI and Console projects.

## Windows
`OsrStudio.Windows` conatins code that is completely specific to the Windows OS.

## Console
`OsrStudio.Console` builds the console application.

## View Core
`OsrStudio.ViewCore` project contains View models and is depended on by the UI project.

## UI
`OSR Studio` is a WPF project containing the UI.

## Other
The remaining projects add specific features like Imgur, SharpAvi, FFmpeg, etc.
