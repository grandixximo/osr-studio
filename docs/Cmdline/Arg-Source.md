# Using the source Argument

## Desktop
Use the `desktop` parameter to capture the entire Desktop **(Default)**.
Works with both `osr-cli start` and `osr-cli shot`.
This is the default option, so is as good as not using this option.

e.g.

```
osr-cli start --source desktop
```

## Region
Use Left, Top, Width and Height resp. as comma separated values to represent the region to capture.
Works with both `osr-cli start` and `osr-cli shot`.
The dimensions of the region must be even. If not, they are decreased by 1 as required.

e.g.

```
osr-cli shot --source 100,100,300,400
```

## Screen
Use `screen:<index>` as the argument. `index` is a zero-based index identifying the screen.

Works with both `osr-cli start` and `osr-cli shot`.

You can use `osr-cli list` to check screen indices.

e.g.

```
osr-cli start --source screen:1
```

## No Video
Use `none` for No Video.

Available only with `osr-cli start`.

Can be used for audio only recording.

e.g. Record only the speaker output.

```
osr-cli start --source none --speaker 0
```

## Window
Use `win:<hWnd>` as the argument. `hWnd` is handle of the window.

When using with `osr-cli start`, the window handle must be in the `osr-cli list` output.

You can use `osr-cli list` to check visible window handles.

## Webcam
Use `webcam` as the argument to capture only the webcam.
Can only work with `osr-cli start`.

Use the `--webcam` argument to set the webcam.
You can use `osr-cli list` to check available webcams.

e.g.

```
osr-cli start --source webcam --webcam 0
```
