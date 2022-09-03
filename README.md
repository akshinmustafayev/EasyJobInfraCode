<img src="assets/header.svg"/>

[![EasyJobInfraCode](https://img.shields.io/badge/EasyJobInfraCode-blue)](https://github.com/akshinmustafayev/EasyJobInfraCode)
<a href="https://img.shields.io/github/license/akshinmustafayev/EasyJobInfraCode">
  <img src="https://img.shields.io/github/license/akshinmustafayev/EasyJobInfraCode" alt="License" />
</a>
<a href="https://img.shields.io/tokei/lines/github/akshinmustafayev/EasyJobInfraCode">
  <img src="https://img.shields.io/tokei/lines/github/akshinmustafayev/EasyJobInfraCode" alt="Total lines" />
</a>
<a href="https://img.shields.io/github/downloads/akshinmustafayev/EasyJobInfraCode/total">
  <img src="https://img.shields.io/github/downloads/akshinmustafayev/EasyJobInfraCode/total" alt="Downloads" />
</a>
<a href="https://img.shields.io/github/stars/akshinmustafayev/EasyJobInfraCode?style=social">
  <img alt="GitHub repo file count" src="https://img.shields.io/github/stars/akshinmustafayev/EasyJobInfraCode?style=social">
</a>
<a href="https://img.shields.io/github/contributors/akshinmustafayev/EasyJobInfraCode">
  <img alt="GitHub repo file count" src="https://img.shields.io/github/contributors/akshinmustafayev/EasyJobInfraCode">
</a> 

---

## :newspaper: Description
<img src="assets/logo.png" height="20" width="20"/> EasyJobInfraCode - is a Lightweight automation tool, performs administrative tasks using __yaml__ playbooks.

## :page_with_curl: Examples
```yaml
Name: Automation Workflow Example 1
Description: Copies files from one directory to another
Author: Akshin Mustafayev
Url: https://github.com/akshinmustafayev/EasyJobInfraCode
Version: 1.0
Copyrigth: Akshin Mustafayev
Steps:
  - ActionType: CopyFolder
    FolderSource: D:\somefolder1
    FolderDestination: D:\somefolder2
```
```yaml
Name: Automation Workflow Example 2
Description: Reads service name from file and then starts it
Author: Akshin Mustafayev
Url: https://github.com/akshinmustafayev/EasyJobInfraCode
Version: 1.0
Copyrigth: Akshin Mustafayev
Variables: [ "$var1" ]
Steps:
  - ActionType: InvokePowerShellScript
    Script: |
        $fileContent = Get-Content D:\1.txt
        Write-Host $fileContent
    PowerShellArguments: -NoLogo
    OutBuffer: $var1

  - ActionType: Service
    Name: $var1
    Action: Start
```


<img src="assets/footer.svg"/>
