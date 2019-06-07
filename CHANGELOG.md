# Change Log

## 1.0.9
 - CardPart page type added to the page wizard
 - Misspelled word in object wizard selection fixed

## 1.0.8
 - Error handling added to DevTools language server client to prevent crashes on unsupported platforms (Mac, Linux)

## 1.0.7
 - New al objects wizards:
   - Page Wizard
   - XmlPort Wizard
   - Report Wizard
   - Query Wizard
   - Enum Wizard
   - Enum Extension Wizard
 - Github issue #31 fixes - Code action "Add multiple fields": Don't always surround with quotes

 Thank you rvanbekkum for reporting issue #31

## 1.0.6

 - "Add multiple fields" code action added to
   - "group" and "repeater" elements on pages
   - "group", "repeated", "addfirst", "addlast", "addafter" and "addbefore" elements on page extensions
   - "dataitem" element on reports
   - "dataitem" element on queries
 - Updated list of referenced nodejs modules
 - Github issue #28 fixed - <ObjectType> placeholder for camel-cased folder names. 
 - Github issue #29 fixed - Bug with generating objects from AL Object Browser without selecting row

 Thank you rvanbekkum for reporting issues #28 and #29

## 1.0.5

 - Extension name change to 'AZ AL Dev Tools/AL Code Outline'
 - Extension README update
 - New commands:
   - "AZ AL Dev Tools: Show All Project Symbols"
   - "AZ AL Dev Tools: Show Project Symbols without Dependencies"
   - "AZ AL Dev Tools: Show Action Images"
 - If the latest version of Microsoft AL Extension is active, "Run Object in Web Client" functionality uses new ability to start debugger without compiling and deploying solution to the server 
 - App files and project symbols processing moved to c# language server to improve performance

## 1.0.4

 - Github issue #25 fixed - Incorrect sorting of objects in AL Object Browser
 - Github issue #26 fixed - Generating Page Extensions no longer working
 - Github issue #27 fixed - Name Filter in 'TreeView' Object Browser filters on Type + Name

 Thank you rvanbekkum for reporting these problems 

## 1.0.3

 - It is now possible to switch AL Object Browser view between list view looking like Nav Object Designer and tree view looking like C# class browser in Visual Studio

## 1.0.2

 - Nav 2018 AL Extension support added to the language server
 - Active AL extension detection when multiple version of the extension are installed
 - Empty outline view at startup bug fixed

## 1.0.1

 - Language server target .net framework changed from 4.7.2 to 4.7

## 1.0.0

 - Code outline symbols and app files processing moved to external C# language server
 - New file parser can detect function types and display different icons for them in the outline view
 - Preview of the new AL Symbols Browser, it can be turned on in the settings by switching alOutline.enableFeaturePreview to true 

## 0.0.15

 - Another great rvanbekkum contribution -it is now possible to select multiple rows in AL Object Browser by clicking on them with Ctrl or Shift key pressed or by using Ctrl+A keyboard shortcut. Object code generators are run for all selected objects.

## 0.0.14

 - Most of file and object name patters removed and redirected to functionality that has already beed implemented by Waldo in  crs-al-language-extension. These settings have been removed:
    * `alOutline.extensionObjectFileNamePattern`: default file naming pattern for new extension objects
    * `alOutline.extensionObjectNamePattern`: default naming pattern for new extension objects
    * `alOutline.fullObjectFileNamePattern`: default file naming pattern for new full objects

## 0.0.13

- Small object wizard bugfix by rvanbekkum. Pressing ESC during object id selection was not closing new object wizard.

## 0.0.12

- new functionality allowing to automatically save generated object on disk in project folder has been implemented by rvanbekkum. These settings allowing to setup that functionality have been added:
    * `alOutline.autoGenerateFiles` setting : automatically generate files for newly created objects
    * `alOutline.autoGenerateFileDirectory`: the default directory to create files in, relative to the root directory (e.g., \"Source\<ObjectType\>\")
    * `alOutline.autoShowFiles`: automatically show any newly created files in the editor
    * `alOutline.promptForObjectId`: when generating a new object, ask the user to input the object ID.
    * `alOutline.promptForObjectName`: when generating a new object, ask the user to input the object name
    * `alOutline.promptForFilePath`: when generating a new file, ask the user to specify a path relative to the root of the project-folder
    * `alOutline.stripNonAlphanumericCharactersFromObjectNames`: always strip non-alphanumeric characters from generated object names

## Thank You rvanbekkum for these modifications!  
 
## 0.0.11

- another great modification added by rvanbekkum - filtering in AL Object Browser. Thank You!

## 0.0.10

- new option to create page and table extensions from AL Object Browser added by rvanbekkum
- fix to make outline work with symbols returned by the latest version of AL Language extension
- github issue [#8 Go to definition no more working](https://github.com/anzwdev/al-code-outline/issues/8) - go to definition function removed from AL Object Browser because of changes in Microsoft "AL Language" extension.   
- obsolete vs code text document provider api used by APP file viewer replaced by webviews
- missing DotNetPackage and Enum object types added to the AL Object Browser 
- AL outline crash on documents without symbols fixed 

### Thank You
 - rvanbekkum for page and table extension functionality 
 - KristofKlein for reporting issue #8

## 0.0.9

- github issue [#7 AL Outline stops working after Create a Page](https://github.com/anzwdev/al-code-outline/issues/7) fixed 
- github issue [#5 Seems to be slow or out of sync](https://github.com/anzwdev/al-code-outline/issues/5) fixed
- github issue [#2 Closing vs code settings does not refresh code tree](https://github.com/anzwdev/al-code-outline/issues/2) fixed

Thank you SirBETE, martinhocosta88 and GreatScott000 for posting your issues and questions. 

## 0.0.8

- Run in Web Client action added to the context menu of table, page and report symbols in symbols tree view.
- Run in Web Client action added to the context menu of table, page and report objects in AL Object Browser
- Default Web Client port setting (alOutline.webClientPort) added to the extension settings  

## 0.0.7

- Bug causing AL Language Extension to returnd incorrect SymbolKind value has been fixed by Microsoft so we no longer need workaround fixing it in AL Code Outline, but it should still work if developers are using older version of AL Language Extension

## 0.0.6

- Code refactoring
- New code generators added to the context menu of table tree node:
    - report code generator
    - xmlport code generator
    - query code generator
- App packages symbol browser

## 0.0.5

- Page code generator assigns ApplicationArea to all fields, default value (All) can be changed using alOutline.defaultAppArea setting
- Page code generator assigns UsageCategory to list pages, default value (Lists) can be changed using alOutline.defaultListUsageCategory setting

## 0.0.4

Symbols tree was empty when user switched view to OUTPUT or DEBUG CONSOLE. Symbols were visible for PROBLEMS and TERMINAL views only.

## 0.0.3
 
- First key in a table has different, primary key icon now 
- AL Outline view is always visible now and displays code structure for file types in the project (i.e. javascript, css, html)
- Right click on 'Table' node in symbols tree shows context menu with 2 new options: 'Create Card Page' and 'Create List Page'

## 0.0.2
- README changes

## 0.0.1
- Initial release