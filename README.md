# CalScreener Converter

## Overview

The data models used by the two versions differ significantly. In CalScreener 2.0, each calibration is stored as an individual XML file, with separate files for different plates and temperatures. In CalScreener 3.1, calibrations belonging to the same device are consolidated into a single JSON file.

During conversion, the tool scans all XML files in the input directory, including any subdirectory, groups them by device ID in the 2.0 XML files, and merges the calibration data into a single CalScreener 3.1 JSON document for each device. All discovered calibrations for that device are included in the generated output file.

A single input folder may therefore contain files for multiple devices. The converter automatically separates them into distinct output JSON files without requiring any manual sorting or preprocessing.

The converter preserves the calibration information available in the source files while restructuring it to match the CalScreener 3.1 schema.

## Requirements

* Windows x64 operating system



## Usage

The converter can be started from a command prompt:

```cmd
Converter.exe [options]
```

Alternatively, the converter can be started by double-clicking `Converter.exe` in File Explorer. In this mode, command-line options cannot be supplied and the converter will run using the default input and output directories.

### Running from Command Prompt

1. Open Command Prompt.
2. Navigate to the directory containing `Converter.exe` using the `cd` command.

Example:

```cmd
cd C:\Users\Daniel\Desktop\symcell\CalScreenerConverter
```

3. Run the converter:

```cmd
Converter.exe
```

### Easy Way to Open Command Prompt in the Correct Directory

An easy way to open a command prompt in the correct location is:

1. Open the folder containing `Converter.exe` in File Explorer.
2. Click the address bar.
3. Replace the current path with `cmd` and press Enter.

A Command Prompt window will open with the current directory already set to the converter's location.

Alternatively, right-click inside the folder containing Converter.exe and select Open in Terminal. A terminal window will open with the current directory already set to the selected folder






## Options
| Option                  | Description                                          |
| ----------------------- | ---------------------------------------------------- |
| `--input <path>`        | Input directory containing legacy calibration files. |
| `--output <path>`       | Output directory for converted files. Will create it if it doesnt exist              |
| `--variant <name>`      | Use the specified variant for all converted files.   |
| `--ignore-empty-variant`| Defaults every variant to 6210 without asking for each empty variant field.|

If no input or output directories are specified, the tool uses its default directories relative to the executable location.


## Variant Handling

In CalScreener 3.1 device settings, the device identifier includes a variant, which is also embedded in the file name.

```
Symcel.CalSys.CalScreener-USB-6210-35675278
```

In this example:
- Device serial: `35675278`
- Variant: `USB-6210`


The corresponding information is not available in the CalScreeners 2.0 device settings XML files and therefore cannot be determined automatically during conversion.

The Variant refers to the nidaq card variant the converted calibration file will be read by. Always starting with the prefix USB- (which doesn't need to be written manually).

The converter supports the following approaches:

1. Provide a variant for all converted devices using the `--variant` argument.
2. Allow the variant fields to automatically default to USB-6210 using `--ignore-empty-variant`.
3. Enter a variant interactively when prompted during conversion.

When running interactively, the converter will prompt for the variant value only. For the example identifier above, the value to enter would be:
```
6210
```

Which would give the Variant value `USB-6210`

Pressing Enter without entering a value will default the variant to 6210. See the Example for an example of Variation Handling.


## Example
Running 
```cmd
Converter.exe --input C:\Users\Daniel\Downloads\jsonochxml --variant 6210
```

will read from the specified folder, and write the output to the standard Output folder, and predefining that all jsons created shall have the variant USB-6210, and thus the program will not ask for each group it converts.

However running 

```cmd
Converter.exe --output C:\Users\Daniel\Downloads\newOutPutFile
```

will read the standard input file and save to the specified file, which will be created if it doesn't exist. The program will ask for the Variant for each group:

```bash
Converter.exe --output C:\Users\Daniel\Downloads\newOutPutFile
Loaded 2 groups of legacy settings for conversion.
Variant for group 'NI-DAQmx-356752748': USB-Variant1
Variant for group 'NI-DAQmx-35675278': USB-
Converted 2 groups of legacy settings.
Written converted settings to C:\Users\Daniel\Downloads\newOutPutFile\Symcel.CalSys.CalScreener-USB-Variant1-356752748.json
Written converted settings to C:\Users\Daniel\Downloads\newOutPutFile\Symcel.CalSys.CalScreener-USB-6210-35675278.json
```
In the example XML files where copied and received new id to showcase the grouping.

## Assumptions and Constraints

The following assumptions and constraints were made during implementation due to limitations in the CalScreener 2.0 format and incomplete migration requirements.


### 2.0 File Naming and metadata assumption

All CalScreener 2.0 XML files are expected to follow a naming convention beginning with:

`
NI-DAQmx-<serial number> <temperature> <plateName>`

For grouping to work the fields:
* UToPFactor
* SkipLowCount
* SkipHighCount
* SamplingInterval
* HardwareSwitchTime
* MeasurementMode
must be identical between the grouped XML files, or the program will fail converting them.

Although for groups with just a single calibration file (i.e there is just one file with a specific serial number), if that file doesn't have temperature and platename it will default to 37 titan. 

Acceptable platenames for the converter (not case sensitive) grouped by 4.1 plate names:
* calPlate Pro glass
    * glas
    * calPlatePlusGlass
* Standard
    * standard
    * titan
* calPlate Pro
    * titanXtalk
    * calPlatePlus


#### CalibrationPoints
CalibrationPoints are not perserved in the conversion, as instructed by Andreas.


#### Number of Samples
`NumberOfSamples` is defaulted to 120, based on inspection of the Code. This might be wrong and maybe should have been kept at `null`, however the code defaulted it to 120
in support/2.0 and support/3.1 so assumption that it should always be 120 from an old file as it never says different was made.


### CrosstalkCalibrationPoint
The following rules describe how CrosstalkCalibrationPoints are generated during conversion. These rules are intended to improve traceability and allow verification of whether issues originate from the conversion process.

The 8 first and last wells do not have any crosstalk calibration points. Which has been confirmed by Andreas.

#### CrosstalkCalibrationPoints Comment
Each CrosstalkCalibrationPoint Comment is automatically generated to:
`AUTO Generated Comment: This CrosstalkCalibrationPoint was generated from a version 2.0 calibration file`. This is to give transparancy in case of something going wrong with the CalScreener to easily see if it is caused by a conversion.

#### CrosstalkCalibrationPoints CalibrationId

A `CalibrationId` is generated during conversion and assigned to all CrosstalkCalibrationPoints originating from the same XML file.

All wells (8–39) derived from a single XML file share the same `CalibrationId`.

#### CrosstalkCalibrationPoints Timeoffset
Each CrosstalkCalibrationPoint `TimeOffset` is set to the timestamp at which the conversion was executed.

#### CrosstalkCalibrationPoints GainFactors
The `GainFactor` is mapped directly based on the active gain value for the corresponding well index.

All non-active gain entries for other wells are set to zero.

#### CrosstalkCalibrationPoints TimeConstants
All `TimeConstants` values are set to zero during conversion.

#### CrosstalkCalibrationPoints IsIncluded
The field `IsIncluded` is always set to true during mapping 

### WellSettings

`Gain` and `Offset` are defined in each CalScreener 2.0 XML file as part of the well settings.

When multiple XML files are merged into a single device in CalScreener 3.1, these values become ambiguous across files.

#### Gain

`Gain` values are not consistent across multiple XML files for the same device after merging.

Because of this ambiguity, per-file gain values are not preserved.

Instead, `Gain` is normalized to `1` for all wells in the output.

Gain information used for computation is represented separately in `CrosstalkCalibrationPoints`.


#### Offset

`Offset` is present in the source XML but is not used in the merged model. It is therefore set to `0` for all wells in the output.

#### ReferenceWellIndex

`ReferenceWellIndex` is  always set to `-1`.