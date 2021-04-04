<img src="https://github.com/GittyMac/Zipper/blob/master/ZipperWin32/Square310x310Logo.scale-100.png?raw=true" alt="Zipper Logo" width="120"/>

# Zipper
An open source, modern archive utility.

![Screenshot of Zipper](https://user-images.githubusercontent.com/28932047/113515702-10606680-9544-11eb-95f7-626ea41ed631.png)

# What does it do?
Zipper can open, archive, and extract files. You can double click to open single files, or drag and drop any file to extract. Directories are currently not handled the best way. Thanks to .NET 5's WinForms implementation, it uses the full modern Windows file and folder selection dialogs.

# Problems
Here are some problems that Zipper currently has.
* Directories are handled fairly bad.
* The UI could still be improved a fair bit.
* There are no icons for the files.
* The extraction can freeze the program, due to it not being async.
* The progress bar doesn't work the best. 

# Building
This app requires .NET 5, and it should compile like any other WinForms app using .NET 5.
