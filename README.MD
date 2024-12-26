# Android Logcat to Readable Log Converter

A simple .NET project that converts log files generated from Android Studio (`*.logcat`) to a more readable log file format (`*.log`).

## Features

- Converts raw logcat output (`*.logcat`) to a readable and structured log file (`*.log`).
- Transform unreadable logcat files into human-friendly log files.
- Lightweight and straightforward to use.

## Installation

1. Clone the repository to your local machine.
   ```bash
   git clone https://github.com/yourusername/yourrepositoryname.git
   ```
2. Open the project using a .NET development environment, such as Visual Studio.
3. Build the project to generate the executable file.

## Usage

1. Generate a `.logcat` file using Android Studio:

   - Open Logcat in Android Studio.
   - Export the logs as a `.logcat` file.

2. Create directory in project directory `InputLogs`

3. Provide the `.logcat` file as input to this program and place in the directory `InputLogs`.

4. The program will process the file and produce a readable `.log` file in the specified output directory.

## Example

### Input (`example.logcat`):

```
{
  "metadata": {
    "device": {
      "deviceId": "<device-id>",
      "name": "<device-name>",
      "serialNumber": "<device_serial>",
      "isOnline": true,
      "release": "<android-version>",
      "sdk": <sdk>,
      "model": "<device-model>",
      "isEmulator": false
    },
  },
  "logcatMessages": [
    {
      "header": {
        "logLevel": "WARN",
        "pid": 0,
        "tid": 1,
        "applicationId": "com.example.app",
        "tag": "ResourcesCompat",
        "timestamp": {
          "seconds": 1734418485,
          "nanos": 168000000
        }
      },
      "message": "\tat android.os.Looper.loopOnce(Looper.java:230)\n\tat android.os.Looper.loop(Looper.java:319)\n\tat android.app.ActivityThread.main(ActivityThread.java:8919)\n\tat java.lang.reflect.Method.invoke(Native Method)\n\tat com.android.internal.os.RuntimeInit$MethodAndArgsCaller.run(RuntimeInit.java:578)\n\tat com.android.internal.os.ZygoteInit.main(ZygoteInit.java:1103)"
    },
  ]
}
```

### Output (`example.log`):

```
[D] 12-26 14:35:12.345  [TAG]: This is a debug log.
[I] 12-26 14:35:13.456  [TAG]: This is an info log.
[E] 12-26 14:35:14.567  [TAG]: This is an error log.
```

## Contributing

We welcome contributions! If you'd like to improve the project or fix bugs:

1. Fork the repository.
2. Create a feature branch.
3. Commit your changes and submit a pull request.

## License

This project is licensed under the MIT License. See the [LICENSE](LICENSE) file for details.
