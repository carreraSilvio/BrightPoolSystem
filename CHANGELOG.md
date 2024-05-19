## [1.7.0] - 2024-04-29

### New
* Changed execution order from -10 to -60
* Changed spawn point gizmo to use actual distance set instead of fixed value

## [1.6.0] - 2023-07-22

### New
* Showing error if given poolable id is invalid
* Added option to spawn from specific spawn point

## [1.5.0] - 2022-12-27

### Fix
* Code inconsistencies

## [1.4.0] - 2022-09-07

### New
* Added warning when poolable not found
* Changed pool actions to include the object acquired/released
* Added option to release all objects from a pool

### Remove
* Removed unused classes and properties

## [1.3.0] - 2021-12-20

### New
* Better null checks
* Drawing gizmos with spawn point controller

## [1.2.0] - 2021-06-06

### New
* Removed internal PoolTracker class to reduce complexity

### Fix
* Typo in Aquire
* Updated method description

## [1.1.0] - 2021-04-04

### Fix
* Added null check for onAquire event
* Made OnAquire and OnRelease methods protected instead of internal

## [1.0.0] - 2020-03-27

### New
First version of the package