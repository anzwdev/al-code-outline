# Change Log

## 3.0.59
 - Issue #545 - Object Definition not available

Thank you
 - mjelle9 and DomKaz for reporting issue #545

## 3.0.58
 - Issue #548 - AL Page Wizard for Cust. Ledger Entry

Thank you
 - tonyabriccomeske for reporting issue #548

## 3.0.57
 - Issue #419 - Xmlport / XmlPort sync with LinterCop
 - Issue #520 and #507 - Namespaces support
 - Issue #521 - Apply Default API settings when creating an api query
 - Issue #530 - Code Cleanup slow 
 - Issue #532 - Code Action "Add multiple fields" doesn't work when SourceTable includes a dot
 - Issue #534 - Text Decorators does not work correctly when app.json is saved as UTF8-BOM
 - Issue #535 - Separate 'Sort Triggers' action #535
 - Issue #536 - [TEST] is not converted to [Test] when running "fix Case"
 - Issue #537 - "Run Code Cleanup on Uncommited Files" requires one click per file
 - Issue #538 - Issues running cleanup: "Unexpected StringComparison type: 'CurrentCultureIgnoreCase'. Only 'Ordinal' and 'OrdinalIgnoreCase' are supported."
 - Issue #543 - Major slowdown in "Run Code Cleanup on Uncommitted Files in the Active Project"

Thank you
 - ChrisBlankDe, cegekaJG for reporting and discussing issue #419
 - jorispoppe for reporting issue #521
 - dsaveyn and fvet for reporting issue #530
 - achim-t for reporting issue #532
 - zabcik for reporting issue #534
 - fvet for reporting issue #535
 - guidorobben for reporting issue #356
 - NKarolak for reporting issue #537
 - jwikman for reporting issue #538
 - cegekaJG for reporting issue #543

## 3.0.56
 - Issue #507 - Namespaces quick fixes and commands
 - Issue #514 - Code Analyzers: most AppSourceCop rules appear twice
 - Issue #516 - Add permissions functionality seems to be broken.
 - Issue #519 - Convert Object Ids to Names does not convert CodeunitID in AddAction of ErrorInfo
 - Issue #520 - Namespaces support added
 - VS Code api references updated to the latest version of VS Code
 - Language server communication updated and redesigned
   - Old vscode language server client TypeScript libraries replaced by the latest vscode jsonrpc library
   - Custom c# language server implementation removed and replaced by Microsoft StreamJsonRpc library
 - Language server upgraded to .net 8

Thank you
 - NKarolak for reporting issue #514
 - guidorobben for reporting issue #516
 - pri-kise for reporting issue #519

## 3.0.55
 - Issue #515 - Empty path name is not legal error fix

Thank you
 - RadoArvay, bokhimlin and tillrouvier for reporting issue #515

## 3.0.54
 - Idea #501 - If Expression in a Case-statement is an option/enum list all members
 - Issue #504 - Empty section is removed
 - Issue #505 - Sort procedures skips regions
 - Issue #506 - New commands: Add missing ending dot to ToolTips
 - Merge pull request #503 by fvet - readme update
 - Idea #509 - Add region around namespaces
 - Feature request #510 - Remove semicolon from procedure declaration 

Thank you
 - Floris-G for idea #501
 - fvet for reporting issue #504 and #505 and pull-request #503
 - Arthurvdv for reporting issue #506 and #510

## 3.0.53
 - Issue #453 - One Statement Per Line bugfix - incorrect line break before "until"

Thank you
 - fvet for finding bug in the issue #453

## 3.0.52
 - Issue #440 - [Suggestion] Syntax tokens in the Document Syntax Visualizer 
 - Issue #453 - One Statement Per Line bugfix - line break inside "else begin"
 - Issue #493 - Command to collapse empty brackets into a single line - project processing bugfix
 - Issue #495 - Trim Spaces In Enum Captions/Values

Thank you
 - jhoek for idea #440
 - NKarolak for finding buig in the issue #453
 - rvanbekkum for finding bug in the issue #493
 - mjmatthiesen for reporting issue #495

## 3.0.51
 - Issue #453 - New code cleanup action - One Statement Per Line
 - Issue #476 - Skip objects with full direct InherentPermissions (QuickFix "Add all application objects permissions")
 - Issue #491 - Variable sorting with "ReportFormat"
 - Issue #492 - FindProject method crashes sometimes if multi-folder workspace contains non-al project
 - Issue #493 - Command to collapse empty brackets into a single line

Thank you
 - fvet for reporting idea #453
 - ernestasjuska for reporting issue #476
 - ChrisBlankDe for reporting issue #491
 - DavidFeldhoff for reporting and helping investigate issue #492
 - Nicholas Jackson for the Twitter idea #493 and Tine Staric and Krzysztof Bialowas for making sure that I've received it

## 3.0.50
 - Extension recompiled for BC23

## 3.0.49
 - Conditional directives parser elif support bugfix

## 3.0.48
 - Issue #482 - Code cleanup removes compiler directives partly
 - Issue #483 - Fix identifiers and Case does not fix internal
 - Conditional directives parser and editor decorations

Thank you
 - NKarolak and fvet for reporting issue #482
 - guidorobben for reporting issue #483

## 3.0.47
 - Issue #470 - Add "DataClassification" as table property to the "New Table Wizard"
 - Issue #472 - CodeCompletion - Do not Ignore Prefix in variable names
   - new alOutline.keepVariableNamesCompletionAffixes setting added
 - Issue #473 - When reusing tooltip from another page, include the tooltip comment
 - Issue #474 - Remove with usage on codeunits with TableNo
 - Linux version

Thank you
 - rvanbekkum for reporting issue #470
 - Thyme1 for reporting issue #472
 - jhoek for reporting issue #473
 - smartinez-io for reprting issue #474

## 3.0.46
 - Issue #276 - AL Object ID Ninja wizards integration not always works
 - Issue #432 - Remove Redundant DataClassification removes it from table extension fields
 - Issue #444 - Completion provider seems to ignore 'internal' objects
 - Issue #465 - Order triggers according to their natural order
 - Issue #467 - AddMissingParentheses adds parentheses to function references in event subscribers (AL Runtime 11.x)

Thank you
 - SBiernat for reporting problem with task #276
 - jwikman for reporting problems with task #432
 - fvet for reporting issues #444 and #467
 - jhoek for reporting issue #465

## 3.0.45
 - Issue #432 - Remove redundant DataClassification
 - Issue #443 - codeCleanupAction AddApplicationAreas adds Application Area to API Pages
 - Issues #455 and #464 - Unable to resolve resource, Unable to load schema from 'aloutlinesyntax://json/appJsonSyntax.json'
 - Issue #452 - Object definition not available 
 - Issue #460 - Please, don't Remove Empty Lines before table-fields { ... } block (add data classification command)

Thank you
 - jwikman for suggestion #432
 - ChrisBlankDe for reporting issue #443
 - zhaoyun247, salgiza, tscottjendev and TheDenSter for reporting issues #455 and #464
 - DivyaniKhandelwal for reporting issue #452
 - JavierFuentes for reporting issue #460

## 3.0.44
 - Issue #276 - AL Object ID Ninja integration
   - New `alOutline.idReservationProvider` setting with these values available: LocalFiles, ALObjectIdNinja
 - Issues #461 and #462 - Extension not working with AL Language Extenstion v11
   - al.packageCachePath setting is an array of strings now

Thank you
 - vjekob for accepting "AL Object ID Ninja" external API pull request
 - guidorobben and Arthi-Dev for reporting problems with the latest AL compiler

## 3.0.43
 - Issue #451 - README and CHANGELOG disappeared from VS Marketplace
 - Information about AL Object ID Ninja integration removed from the changelog as it is still work in progress

Thank you
 - rvanbekkum for reporting issue #451
 - fvet for checking changelog

## 3.0.42
 - Issue #302 - It is now possible to enable or disable removing quotes around identifiers that don't look like al data types by using `alOutline.fixCaseRemovesQuotesFromNonDataTypeIdentifiers` setting. Default value is true which means that quotes are removed.
 - Issue #431 - app.json encoded with "UTF-8 with BOM" fails
 - Issue #434 - Page layout group(general) has no caption
 - Issue #436 - comment= is not fixed when running "Fix identifiers"
 - Issue #439 - Completion provider for "var" function parameter?
 - Repository structure changed
   - vscode extension typescript project moved to vscode-extension subfolder
   - dotnet language server project moved from separate repository tp language-server subfolder
   - devops workflow building extension added
   - build.ps1 script added - it is used by the workflow, so might not work when run manually
 - PullRequest 446 - Add possibility to modify wizardData from other extensions before calling and showing the wizard.

Thank you
 - JavierFuentes for asking to leave quotes in issue #302
 - jwikman for reporting issue #431
 - guidorobben for reporting issues #434 and #436
 - fvet for reporting issue #439

 - DavidFeldhoff for PullRequest 446

## 3.0.41
 - Issue #6 - AL Outline panel - sort symbols by position, name and type
 - Issue #327 - Use different icons in AL Outline panel
 - Issue #328 - Improvements in AL Outline panel
   - collapse all
   - region support
   - saving tree state
   - more symbols collapsed by default
 - Issue #363 - XmlPorts - Generate headers 
   - new `Generate column headers for CSV export` action available on XmlPort header and XmlPort tableelement when XmlPort format is set to "VariableText"
 - Issue #377 - Don't add captions to the API pages fields
   - new `alOutline.createApiFieldsCaptions` setting added
 - Issue #378 - API Pages : API field name suggestions for 'number' fields (number instead of no)
   - new `alOutline.apiFieldNamesConversion` setting
 - Issue #380 - AL Outline panel support Region Directive in AL (New Feature Request) 
 - Issue #385 - AL Language Server Crashing - temp-al-proxy.al 
   - all references to code creating that temp file removed
 - Issue #410 - Search in Object browser should not be case sensitive
 - Issue #412 - [Request] add permissions to current object
   - new code action on first line of object definition and permission property: "Add permissions to all tables used by this object (AZ AL Dev Tools)"
 - Issue #414 - Action for adding missing DataCaptionFields property and/of DropDown field group (in Active Editor/Active Project)
   - new commands:
     - "Add Table DataCaptionFields to the Active Editor"
     - "Add Table DataCaptionFields to the Active Project"
     - "Add DropDown Field Groups to the Active Editor"
     - "Add DropDown Field Groups to the Active Project"
   - new settings:
     - "alOutline.dropDownGroupFieldsNamesPatterns": array of string patters for table DropDown group fields, you can use "*" for partial name matching
     - "alOutline.tableDataCaptionFieldsNamesPatterns": array of string patters for field names for table DataCaptionFields property, you can use "*" for partial name matching
 - Issue #415 - Variable completion
   - #425 fix should improve suggestions here, but the real problem is caused by the default `"editor.suggest.matchOnWordStartOnly": true,` VS Code setting (ICDimension is treated as single word)
 - Issue #417 - Add all extension objects permissions quick fix should not add redundant permissions
 - Issue #421 - FlowFilters aren't added to API Pages
   - new page wizard step for API pages - flow filters selection
 - Issue #424 - Remove 'with' usage ... does not work with NoImplicitWith compiler flag
 - Issue #425 - Variable Completion including double quotes

Thank you
 - GreatScott000 for reporting issue #6
 - JavierFuentes for reporting issue #327 and #328
 - NKarolak for issue #328 ideas
 - 4dimit for reporting issue #363
 - fvet for reporting issues #377, #378, #415 and #425
 - louagej for reporting issue #380
 - pri-kise for reporting issue #385, #421
 - damse60 for reporting issue #410
 - guidorobben for reporting issue #412
 - jhoek for reporting issue #414
 - ernestasjuska for reporting issue #417
 - M4fin for reporting issue #424

## 3.0.40
 - Issue #411 - Cleanup removes instructions from wizard pages
   - "remove field groups" removed from the functionality
 - Issue #416 - RemoveProjectRedundantAppAreas - Exclude report requestpages (AS0062 error)
 - Issue #418 - Add Page Controls Captions: Skip for ShowCaption = false
   - captions are not added is ShowCaption property is false
   - new "Repeaters" option has been added to the command parameters
   - if sorting propeties on document save is enabled, modified properties are sorted
 - Issue #420 - add new command to the "alOutline.codeCleanupActions": "AddEnumValueCaptions"
 - Issue #422 - Procedure Sorting Criteria - protected and internal procedures sorting fix

Thank you
 - dsaveyn for reporting issue #411 
 - fvet for reporting issue #416
 - NKarolak for reporting issue #418
 - jhoek for reporting issue #420
 - ChrisSteuten for reporting issue #422

## 3.0.39
 - Issue #402 - "Remove empty sections" appears to ignore empty actions section in page extensions (and possibly also in pages?)
 - Issue #404 - [Bug] RemoveProjectRedundantAppAreas - Exclude page/report extensions (AS0062 error)
 - Issue #406 - [BUG] Variable completion provider runs in invalid places
   - completion disabled for triggers and event subscribers parameters and return values
 - Issue #408 - Variable completion suggests non-accessible objects 

Thank you
 - jhoek for reporting issue #402
 - fvet for reporting issues #404 and #406
 - ajkauffmann for reporting issue #408

## 3.0.38
 - Extension recompiled to work with BC21

## 3.0.37
 - Suggestion #387 - BC21 - Field ApplicationArea defaults to page value 
 - Issue #389 - "Add Missing Parentheses": Skip variables with the same name as a record function. 
 - Issue #393 - Make FlowFields read-only, remove DataClassification from FlowFields and FlowFilters
   - new editor and project "Make FlowFields Read-Only..." commands
   - new "MakeFlowFieldsReadOnly" option for Code Clean-up
   - DataClassification command removes property from FlowField and FlowFilter table fields
 - Issue #394 - alOutline.codeCleanupActions: tooltips appear to be "off-by-one"
 - Issue #396 - Account for affixes in the variable name-type completion provider(s)
   - "_" character detection in affixes bugfix
   - new "alOutline.additionalMandatoryAffixesPatterns" setting - affixes patterns where '?' character can be used as "any character"
 - Issue #397 - RemoveEmptyTriggers conflicts with regions 
 - Issue #398 - Add semicolon to end of the new completion provider 
   - ";" is added to variables declaration suggestions as it is always required
   - ";" is not added to parameters and return variable declaration suggestions
 - Issue #399 - ignore duplicates if area is obsolete while using "Find Duplicates" 

Thank you
 - NKarolak for reporting issue #389
 - fvet for reporting suggestion #387 and issues #393, #397
 - jhoek for reporting issue #394
 - rvanbekkum and fvet for reporting issue #396
 - nickgoddard777 for reporting issue #398
 - AndreBreitenbach for reporting issue #399

## 3.0.36
 - Hover and Find References for rule ids in "pragma warning" directives
 - New command "Show warning directives" to browse "pragma warning" directives ordered by rule id
 - Show rules ids dropdown and descriptions on hover when editing rulese files or suppressWarnings property in app.json
 - Variable/Parameters names and types completion
 - Feature Request #388 - Minimum variable sort mode for AA0021
 - Feature Request #392 - Code Clean-up: Remove Empty Triggers and Event Subscribers

Thank you
 - NKarolak for reporting request #388
 - jhoek and DavidFeldhoff for reporting request #392

## 3.0.35
 - Remove unused variables command crash bugfix
 - Bugfix - sometimes "run ... in Active Editor" commands were not working, bug was caused by incorrect case of file names returned by VS Code
 - Idea #371 - Autofix AA0231 ("Do not use the StrSubstNo or string concatenation as a parameter in the Error method")
   - New commands:
     * `Remove StrSubstNo from Error from the Active Editor`: removes StrSubstNo from Error method call from the current editor
     * `Remove StrSubstNo from Error from the Active Project`: removes StrSubstNo from Error method call from the current project
   - New parameter value for code cleanup commands: `RemoveStrSubstNoFromError` 

Thank you
 - jhoek for reporting idea #371

## 3.0.34
 - Issue #365 - Convert Object Ids to Names does not work for profile
 - Idea #366 - Sort Page Customizations on Save
 - Issue #368 - "alOutline.codeActionsOnSaveIgnoreFiles" don't avoid automatic variables sorting
 - Issue #369 - New AL File Wizard - UsageCategory 'None' not respected
 - Issue #372 - Remove 'with' usage does not work if ALPackages folder path has been changed
 - Issue #374 - AL File Wizard - Default settings for API pages:
   * `alOutline.defaultApiPublisher`: Default Api Publisher value for the page wizard
   * `alOutline.defaultApiGroup`: Default Api Group value for the page wizard
   * `alOutline.defaultApiVersion`: Default Api Version value for the page wizard
 - Issue #375 - Add multiple fields to XMLPort shows already added fields
 - Issue #376 - Add multiple fields: sort field properties alphabetically
 - Language server dependencies and target .net framework update

Thank you
 - pri-kise for reporting issue #365 and idea #366
 - FloOM25, Splace13 and frozenbrain for reporting and helping with issue #372
 - JavierFuentes for reporting issue #368
 - fvet for reporting issues #369, #374, #375
 - NKarolak for reporting issue #376

## 3.0.33
 - Issue #352 - Sorting settings ignored by OnDocumentSave version of the command
 - Issue #360 - Exclude API pages from "AddToolTips
 - Issue #361 - Remove unused variables not removing variables for OnAction Trigger

Thank you
 - JavierFuentes for reporting problem with issue #352
 - fvet for reporting problem with issue #360
 - tscottjendev for reporting problem with issue #361

## 3.0.32
 - Suggestion #347 - Cleanup empty lines/sections
 - Issue #352 - Sort the Record variables by VarName, not by TableName
 - Issue #355 - Go to definition (project file or server definition) does not work if object name contains a slash /
 - Issue #356 - Find duplicate code only in active workspace folder

Thank you
 - fvet for reportitng suggestion #347
 - JavierFuentes, rvanbekkum and guidorobben for reporting and discussing issue #352
 - rvanbekkum for reporting issue #355
 - jwikman for reporting issue #356

## 3.0.31
 - Extension recompiled to work with AL compiler v9

## 3.0.30
 - New "Find duplicate code" command
 - Issue #345 - "Add all extension objects permissions" - Skip tables with ObsoleteState = Removed
 - Issue #346 - Error when running Code Cleanup - remove With

Thank you
 - rvanbekkum for reporting issue #345
 - fvet for reportitng issue #346

## 3.0.29
 - Issue #344 - Fix Identifiers and Keywords fails with the latest release
 - Issue #343 - Sort commands do not work with the latest AL Language
 - Issue #341 - Run Code Cleanup fails 

Thank you
 - fvet for reportitng issue #341
 - NKarolak for reporting issue #343
 - ChrisKappe and lvanvugt for reporting issue #344

## 3.0.28
 - Issue #302 - AutoFormat vs. Fix KeyWords casing - Quotes around "Code"
   - It is now possible to enable or disable removing quotes around identifiers that look like al data types by using `alOutline.fixCaseRemovesQuotesFromDataTypeIdentifiers` setting. Default value is false which means that quotes are not removed.
 - Issues #305 and #336 - Not able to find symbols using packageCachePath setting
 - Issue #337 - resourceExposurePolicy is not interpreted properly 
 - Issue #338 - Fix Identifiers and Keywords leads to uncompilable report
 - Pull request #333 by dannoe - Fixed wrong keyboard navigation order and a typo
 - Pull request #334 by dannoe - Polished the layout of the object browser a little bit
 
Thank you
 - pri-kise and JavierFuentes for reporting issue #302
 - Ponch18 for reporting issue #305
 - vody for reporting issue #336
 - aobsgit for reporting issue #337
 - dschulzeOS and JavierFuentes for reporting issue #338/#318
 - dannoe for pull requests #333 and #334
 
## 3.0.27
 - Issue #318 - AZ AL Dev Tools: Fix Identifiers and Keywords Case in the Active Project unquotes identifiers in code that look like keywords 
 - Issue #319 - Duplicating comments on begin..end remove around single statements 
 - Issue #320 - Missing semicolon on begin..end remove around single statements
 - Issue #321 - Show Code Analyzer Rules: Select all by default, add support for '${analyzerFolder}' in analyzer name
 - Issue #323 - Current editor commands crash when run for documents that are not al files
 - Issue #325 - Incorrect label variables sorting
 - Issue #330 - Allow to exclude some of the files from workspace code transformation commands
 - Issue #332 - No Tooltips for ShowCaption = false
 - Pull request #324 by deadmouse - typo fix in the package.json

Thank you
 - ernestasjuska for reporting issue #318
 - dannoe for reporting issues #319, #320
 - rvanbekkum for reporting issue #321
 - deadmouse for reporting issue #323
 - JavierFuentes for reporting issue #325 and #330
 - pri-kise for reporting issue #332
 - deadmouse for pull request #324

## 3.0.26
 - Issue #310 - SortProperties fails in case of extra semicolon
 - Issue #311 - Run code cleanup - specify default parameters
 - Issue #312 - Run Code Cleanup - Folder settings vs workspace settings
 - Issue #314 - Action Images Browser: Tiles
   - New commands added:
     - "AZ AL Dev Tools: Show CueGroup Action Images"
     - "AZ AL Dev Tools: Show CueGroup Fields Images" - "Copy as action" and "Copy as promoted action" context menu items are not available in this case
     - "AZ AL Dev Tools: Show Role Center Action Images"
 - Issue #295 - Typo fix

Thank you
 - fvet for reporting issues #310, #311 #312 and #295
 - rvanbekkum for reporting idea #314

## 3.0.25
 - Issue #249 - Reuse tooltip from other pages
   - new commands `Refresh ToolTips from Dependencies in the Active Editor/Project` to refresh page field tooltips from existing tooltips defined in dependencies
   - page wizard, `add multiple fields` code action and `Add ToolTips to the Active Editor/Project` commands reuse tooltips from other pages
   - in both cases listed above, if there are more different tooltips defined for a field, a first one that logic finds will be used
   - new `Reuse tooltip from other pages` code action available on page and page extension fields and on ToolTip property
   - reusing tooltips in page wizard, `add multiple fields` code action and `Add ToolTips to the Active Editor/Project` commands can be disabled by setting `alOutline.doNotReuseToolTipsFromOtherPages` setting to true, source of tooltips can also be limited to selected dependencies using `alOutline.reuseToolTipsFromDependencies` setting
 - Issue #215 - Empty lines at end of new files
   - new setting "alOutline.noEmptyLinesAtTheEndOfWizardGeneratedFiles"
 - Issue #261 - CodeActionsOnSave is terribly slow (AL <> AL code outline)
   - CodeCop warning fixes code actions are disabled by default, they can be enabled by changing "alOutline.enableCodeCopFixes" setting to true
   - "FormatDocument" value has been added to the list of available actions in "alOutline.codeActionsOnSave" setting
 - Issue #275 - Symbols Browser - Go to definition (project file or server definition) - shared file access fix
 - Issue #295 - New "Code Cleanup" commands for workspace, editor and uncommited only workspace files
 - Issue #295 - New "Lock Removed Table Field Captions" commands for workspace and editor
 - Issue #300 - Idea: Fix begin..end (AA0005) warnings in project/editor
 - Issue #301 - PermissionSet wizard - PermissionSet name length cannot be longer than 20 characters
 - Issue #306 - Add MaxLength property to Caption when Permission Set is created
 - Issue #307 - Allow the Syntax Visualizer sash to be moved further
 - Issue #308 - If possible, only show context menu items when relevant
 - New structure of problems reported by CodeCop was preventing CodeCop code action fixes from running

Thank you
 - mjmatthiesen for reporting idea #215
 - fvet for reporting issue #261
 - GreatScott000 for reporting issue #275
 - fvet and dkatson for issue #295 ideas
 - dannoe for posting idea #300
 - rvanbekkum for reporting issue #306
 - jhoek for reporting issues #307 and #308

## 3.0.24
 - Issue #293 - Sorting code-actions as commands for current file and active project
 - Issue #296 - Codeunit Wizard does not copy temporary flags
 - Sort variables crash during variable names list sorting
 - Remove unused variables was not working with triggers

Thank you
 - rvanbekkum for reporting issue #293
 - cosmolange for reporting issue #296

## 3.0.23
 - Issue #294 - Error adding Parentheses to Active Project - issue fix
 - Issue #298 - Parenthesis added when not required, causes compile error

Thank you
 - GreatScott000 for reporting and helping investigate issues #294 and #298

## 3.0.22
 - Issue #294 - Error adding Parentheses to Active Project - include processed file name in the error log

Thank you
 - GreatScott000 for reporting issue #294

## 3.0.21
 - Convert ID to Names fixes
 - new "Add Missing Parentheses to the Active Editor" and "Add Missing Parentheses to the Active Project" commands
 - NullReference error in "Remove Unused Variables" commands

## 3.0.20
 - Issue #290 - Add application areas and captions to page labels
 - Issue #289 - Add Object Captions to Active Project - exclude API pages
 - Issue #226 and #287 - new "Remove Unused Variables from the Active Editor" and "Remove Unused Variables from the Active Project" commands
 - Language server recompiled to work with the latest version of AL extension

Thank you
 - gretanorv for reporting issue #290
 - fvet for reporting issues #287 and #289
 - nikolaysotirov for reporting issue #226

## 3.0.19
 - Issue #285 - Show object name length/character count in AL Object File Wizards
 - Issue #286 - "Fix Identifiers and Keywords Case" does not fix ReportFormat and built-in report method casing

Thank you
 - rvanbekkum for reporting issue #285
 - ernestasjuska for reporting issue #286

## 3.0.18
 - Language server recompiled against the BC 19 version of the AL Language Extension 
 - Issue #277 - Add Table Field Captions > Exclude 'Removed' fields
 - Issue #282 - AZ AL Dev Tools: Fix Identifiers and Keywords Case in the Active Project incorrectly renames dotnet type definitions
 - Issue #283 - ToolTip should end with a dot ('.') 

Thank you
 - fvet for reporting issue #277
 - ernestasjuska for reporting issue #282
 - lvanvugt for reporting issue #283

## 3.0.17
 - Issue #278 - AZ AL Dev Tools: Fix Identifiers and Keywords Case in the Active Project incorrectly renames system parts
 - Issue #279 - AZ AL Dev Tools: Fix Identifiers and Keywords Case in the Active Project changes ApplicationArea to Text
 - Issue #280 - AZ AL Dev Tools: Fix Identifiers and Keywords Case in the Active Project does not fix area(...) casing
 - Sugest xml documentation comments for old version of AL extension only as current one can do it
 - Enum extension wizard - suggest first value id from project ranges
 - Do not show FlowFilters on the list if available fields in wizards and "Add multiple fields" code actions
 - "New AL File Wizard" command renamed to "New AL File Wizard (AZ AL Dev Tools)" so people know where to send any change requests or report issues

 Thank you ernestasjuska for reporting issues #278, #279, #280

## 3.0.16
 - Issue #254 - "Add multiple fields" does not open fields list 
 - Issue #271 - FileWizard: ObjectId always zero since last update

Thank you NKarolak and DavidFeldhoff for help with these 2 issues

## 3.0.15
 - Issue #262 - New workspace commands: "Fix case of functions/variables references and keywords"
 - New workspace commands "Convert Object Ids to Names in the Active Editor" and "Convert Object Ids to Names in the Active Project"

Thank you
 - fvet for reporting issue #262

## 3.0.14
 - Dependencies update (github dependabot pull requests)
 - Fields selection in Report Extension Wizard
 - Error handling fixes - missing LogError calls added
 - Packlage loading tries to open it multiple times with delay if file is locked

## 3.0.13
 - Issue #262 - Fix keyword casing in variable data types
 - Issue #265 - AL File Wizards for Permission Set, Permission Set Extension
 - Issue #266 - Add DataPerCompany to AL Table File Wizard
 - New code actions:
   - Sort permissions - sorts entries in the Permissions property, can be run on document save
   - Sort permission sets - sorts entries in the IncludedPermissionSets property, can be run on document save
   - Add all extension objects permissions - adds all objects from the current extension to the Permissions property  

Thank you
 - fvet for reporting issue #262
 - rvanbekkum for reporting issues #265, #266

## 3.0.12
 - New Report Extension Symbols-based Object Wizard by rvanbekkum
 - New Report Extension Wizard by rvanbekkum
 - Making a few ALLangServerProxy functions obsolete and switching to background language server
 - getNextObjectId redesigned to use custom code instead of AL Compiler

 Thank you
  - rvanbekkum for "Report Extension Symbols-based Object Wizard" and "New Report Extension Wizard" pull requests

## 3.0.11
 - another extension unload fix for MacOS
 - Issue #259 - Support Copy-Paste of rows in 'New AL Table Wizard'
 - Issue #262 - Fix keyword casing

Thank you
 - fvet for reporting issues #259 and #262

## 3.0.10
 - Extension unload fixes

## 3.0.9
 - app packages handling fixes

## 3.0.8
 - Issue #256 - Suggestion: Tooltip generation from field's description
 - Issue #257 - Issue with Field Name Area for API page

Thank you
 - mjmatthiesen for reporting issue #256
 - pri-kise for reporting issue #257
 
## 3.0.7
 - Issue #245 - Error during Build : al-lang-proxy\tempalfile.al(1,10) after using "go to definition"
 - Issue #252 - sortVariables / sortProperties with // comments results in invalid objects

 Thank you
  - misacrni for reporting issue #245
  - fvet for reporting issue #252

## 3.0.6
 - Issue #88 - "Show Code Analyzer Rules" - include compiler rules (ALxxxx)
 - Issue #213 - Code Actions On Save - incorrect document formatting
 - Issue #228 - API fields names and captions
 - Issue #244 - Add ToolTip not working correct if ' is used
 - Issue #246 - 'Show Action Images' does not work in a multi-root workspace
 - Issue #247 - Exclude specific object from codeActionsOnSave
 - Issue #250 - ToolTip is added when ToolTipML is already set
 
 Thank you
  - rvanbekkum for reporting issue #88
  - mjmatthiesen and daansaveyn for reporting problems with document formatting after OnSave code actions are run
  - Duffy77 for reporting issue #244
  - fvet for reporting issue #228, #246, #247
  - wbrakowski for reporting issue #250
  
## 3.0.5
 - Issue #194 - Object definition not available - load definition from app file if ShowMyCode is set true
 - Issue #237 - Support Workspace Trust
 - New "azALDevTools.showExtensionLog" command to open extension log file

 Thank you
  - fvet for reporting issue #194
  - lramos15 for reporting issue #237

## 3.0.4
 - Both Windows and Mac language servers use .net 5 now
 - Issue #233 - Use table field caption changed by the table extension
 - Issue #239 - Language server breaks when Table Extension object contains field modification
 - Issue #240 - Run in Web Client in Symbols browser

 Thank you
  - DavidFeldhoff for reporting issue #233
  - pavelichenco for reporting issue #239
  - misacrni for reporting issue #240

## 3.0.3
 - Usage category and ApplicationArea in the report wizard
 - Issue #231 - Modify wizards to create object captions without prefix/suffix
 - Issue #232 - Add multiple fields does not work on page extensions
 - Issue #233 - Table field captions not detected for page extension fields and fields based on record variables
 - Issue #234 - Symbols browser not working when profile object added to the project
 - Issue #235 - Table lookup broken
 - Part of issue #194 - Empty outline, empty list of tables in the wizards

Thank you
 - fvet, jwinberg for reporting issue #194
 - meckerlen and vsd1 for reporting issue #232
 - DavidFeldhoff for reporting issue #233 and comments in #194 
 - mhenricus and jmadrigalTCN for reporting the issue #234
 - gnaaktgeboren for reporting issue #235
 
## 3.0.2
 - "remove with" commands small fixes
 - Issue #223 - Create missing captions for Table, Page, Report, XmlPort and Query objects
 - Issue #227 - "Remove 'with' usage" does not work on TestPages
 - Issue #228 - Add multiple fields for API pages does not respect API field properties
 - Issue #229 - Running al wizards from command line instead of using context menu in Explorer

Thank you
 - DanielGoehler for reporting issue #223
 - lvanvugt for reporting the issue #227
 - fwet for reporting issues #228 and #229

## 3.0.1
 - Issue #222 - Add support for new al object types

## 3.0.0
 - Redesigned language server
 - Add tooltips to fields created by the page wizard
 - Missing tooltips/page field captions commands can now use table field captions from referenced extensions
 - 'Add multiple fields' command on page extension can detect fields already added to the page in all referenced extensions
 - Do not show obsolete or disabled fields in the wizards or 'Add multiple fields' commands
 - Bugfix - existing xml ports fields not detected by the 'add multiple fields' command
 - Issue #190 & #205 - Sort fields by Id or Name in fields selectors (code actions, wizards)
 - Issue #208 - Add tooltip to 'add multiple fields'
 - Issue #218 - Remove prefix/suffix when adding captions or tooltips

Thank you
 - TheDenSter for reporting issue #208
 - DanielGoehler for reporting issue #218
 - Waldo and theschitz for reporting issues #190 and #205

## 2.0.25
 - Issue #210 - Sort table fields
 - Issue #211 - Selecting the last row in filtered data grid shows wrong data

Thank you
  - rvanbekkum for reporting issue 211

## 2.0.24
 - Issue #200 - Suggest fields ids in table and table extension wizards

Thank you
  - mjmatthiesen for reporting issue #200

## 2.0.23
 - Issue #204 - AA0137 - "Unused variable" fixes can break code, add "AZ AL Dev Tools" to all code actions descriptions
 - Issue #207 - ApplicationArea on a page object

Thank you
  - rdebath for reporting the issue #204
  - gunnargestsson for reporting the issue #207  

## 2.0.22
 - Issue #199 - new "Add Page Controls Captions to the Active Editor" and "Add Page Controls Captions to the Active Project" commands.

Thank you
  - gunnargestsson for reporting the issue #199

## 2.0.21
 - Commands "Add Field Captions..." renamed to "Add Table Field Captions..."
 - OnSave code actions fix to work with both "source.fixAll.al" and "source.fixAll" vs code settings 
 - Fix problem with sorting procedure, variables, properties and report columns when file contains regions

Thank you
  - gunnargestsson for reporting OnSave code actions and sorting issues

## 2.0.20
 - New commands "Add Field Captions to the Active Editor" and "Add Field Captions to the Active Project"
 - Issue #197 - "remove with" command error - "Index out of range"

Thank you
  - fvet for reporting issue #197

## 2.0.19
 - New "addDataItemToReportColumnName" setting to include data set name in report column names

Thank you
  - Luc van Vugt for "addDataItemToReportColumnName" functionality suggestion

## 2.0.18
 - Issue #187 - Do not remove numbers from API fields names
 - Issue #188 - Formatting result after using "Remove 'with' usage"
 - Issue #191 - "Remove 'with' usage" still prepends redundant record names to 3rd/later fields

Thank you
  - jwikman for reporting issue #187
  - bvbeek for reporting issue #188
  - dtkb for reporting issue #191

## 2.0.17
 - Issue #181 - Language server recompiled to work with BC 2020 Wave 2 AL compiler
 - Issue #184 - New AL File Wizard: Keep line ending settings (CRLF vs. LF)
 - Issue #186 - "Remove 'with' usage" adds redundant names of Rec *and* other vars to field names
 - Issue #187 - API pages naming and no translation
 - Add 'Rec.' to page fields source in the page wizard and "Add fields" code action

 Thank you
  - pri-kise for reporting issue #181
  - NKarolak for reporting issue #184
  - dtkb for reporting issue #186
  - jwikman for reporting issue #187

## 2.0.16
 - Issue #165 - Restore missing alphanumeric sorting in sort code actions

 Thank you
  - rvanbekkum for reporting issue #165

## 2.0.15
 - New "Remove 'with' usage from the Active Editor" command for BC 17

## 2.0.14
 - Sort code actions logic moved to .net language server
 - Issue #165 - Sort procedures code action removed from procedure level
 - New "Remove 'with' usage from the Active Project" command for BC 17

 Thank you
  - rvanbekkum for reporting issue #165

## 2.0.13
 - Issue #170 - Missing sorting by type name added

## 2.0.12
 - Issue #170 - Problem using multiple codeactions on save

Thank you ti-jalopez and StefanMaron for your comments about issue #170 

## 2.0.11
 - Issue #176 - Do not add DataClassification to flow fields and flow filters

 Thank you
  - apoyas for reporting issue #176

## 2.0.10
 - Issue #176 - Add DataClassification to the Active Editor/Project

 Thank you
  - apoyas for reporting issue #176

## 2.0.9
 - Issue #169 - Enum Extension is missing from Object Types
 - Issue #170 - Problem using multiple codeactions on save
 - Issue #172 - IDs missing for EnumValue in symbols tree
 - Issue #173 - Extends property is empty for EnumExtension and TableExtension symbols
 - Simple page extension wizard by rvanbekkum

 Thank you
 - LucVanDyck for reporting issue #169
 - ti-jalopez for reporting issue #170
 - rvanbekkum for page extension wizard pull request and reporting issues #172 and #173

## 2.0.8
 - Issue #147 - Small functionality fix - symbols-based wizards suggest object id but allow user to change it

## 2.0.7
 - Issue #147 - Set the default object ID suggested by the symbols-based wizards to the next available object ID 
 - Issue #158 - Add "follow cursor" functionality to the AL Outline Panel

 Thank you
 - rvanbekkum for suggesting enhancement #147 
 - NKarolak for suggesting enhancement #158

## 2.0.6
 - Issue #153 - Hide content of custom editor and show warning if document cannot be parsed
 - Issue #162 - Table Wizard: Mark PK fields
 - Issue #164 - Can't Generate any files through the wizard
 - Boolean column type support added to the data grid
 
 Thank you
 - NKarolak for reporting issue #162
 - jigsawmegs for reporting issue #164

## 2.0.5

 - Issue #131 - "AL Outline" panel is blank - legacy Nav2018 version of the language server added
 - Issue #154 - Add tooltips to custom json editors
 - Issue #155 - AL Page wizard: Adding a new tab clears previously selected fields
 - Issue #156 - AL Object browser doesn't retain set filters
 - Issue #160 - Table Wizard: switch Length and Data Classification columns
 - WebViews css refactoring, moving duplicated entries to the default.css file

 Thank you
 - LucVanDyck for reporting issues #155 and #156
 - B0uMe for reporting issue #131
 - NKarolak for reporting issue #160

## 2.0.4
 - Issue #140 - Add Tooltips: caption instead of field name
 - Issue #142 - Ruleset editor - add rules lookup
 - Issue #143 - Use ruleset editor for file with filename "ruleset.json"
 - Issue #144 - AppSourceCop.json Visual Editor: "unknown" property (for "publisher" property)
 - Issue #145 - Option to make the AL Object Browser the default 'editor' for .app files
 - Issue #148 - Add dot at the end of tooltip
 - Issue #149 - Missing second quote in <returns name="ReturnVariableName"> in generated documentation comment
 - Issue #152 - SymbolsService does not load Symbols with AL Language version 3.x
 - Autocomplete drop down can display value descriptions
 - Table extension wizard added by rvanbekkum

 Thank you 
 - LucVanDyck for reporting issue #140
 - rvanbekkum for reporting issues #143, #144, #145 and #149 and your pull request with issue #149 fixes and table extension wizard
 - DavidFeldhoff for reporting issue #152

## 2.0.3
 - app.json editor
 - RuleSet editor
 - AppSourceCop.json editor
 - Issue #133 - Documentation comment does not add <returns> for named return value
 - Issue #134 - "Show All Project Symbols" does not show "Base Application" and "System Application" objects if app uses the "application" property to declare these dependencies
 - Issue #135 - Sort Properties crashes document under specific circumstances

 Thank you Arend-Jan Kauffmann for the idea of app.json editor, rvanbekkum for reporting issues #133 and 134 and DavidFeldhoff for reporting issue #135

## 2.0.2
 - Get full syntax tree api bugfix

## 2.0.1
 - Default field tooltip demplate updated
 - "Add ToolTips to the Active Editor" screenshot added to the documentation 

## 2.0.0
 - New version of the language server with .net core and Mac support
 - "Add ToolTips to the Active Editor" and "Add ToolTips to the Active Project" commands by StefanoPrimo

  Thank you StefanoPrimo for your contribution

## 1.0.49
 - Github issues #118 and #131 - Symbols not visible when using Microsoft AL Extension from Nav 2018

  Thank you skkulla and B0uMe for reporting your problems

## 1.0.48
 - Github issue #128 - "Add multiple fields" inserts fields above parent element properties
 - Github issue #129 - Issues with "Add Parenthesis" code fix on document save
   - Code fix is no longer created if "alOutline.fixCodeCopMissingParenthesesOnSave" setting is set to false as Microsoft AL Extension can now create the same fix
   - If "alOutline.fixCodeCopMissingParenthesesOnSave" is set to true, code fix will still be created and run on document save as Microsoft version of the fix cannot do it.
   - Fix is created for document save action only, so there is just one fix for whole file instead of separate fixes for each missing parenthesis.  
 - Github issue #121 - New AL File Wizard: sort properties
 - Page wizard was ignoring entered "Application Area" for fields
 - Github issue #123 - Problems with azALDevTools.showDocumentSymbols command for files other than al
 - Github issue #126 - AL New Table Wizard - Choose Enum Type

 Thank you NKarolak for reporting issues #128 and #121, fvet for the issue #129, HuFBH for the issue #123 and rvanbekkum for the issue #126

## 1.0.47
 - Github issue #122 - codeActionsOnSave ignored 
 - "Sort Procedures" code action fixes by David Feldhoff

 Thank you NKarolak for reporting the issue #122 and David Feldhoff for fixing "Sort Procedures" code action

## 1.0.46
 - Github issue #117 - "New Table Wizard: missing length"
 - Github issue #119 - Adopt VS Code's 'asWebviewUri' API

 Thank you NKarolak for reporting the issue #117 and VS Code Team (Matt Bierner) for issue #119

## 1.0.45
 - AL Compiler libraries loader updated to support the latest version of the compiler

## 1.0.44
 - SyntaxTree visualizer crash fix

## 1.0.43
 - "Sort variables" code action fixes
 - Core refactoring
 - Extension recreated using the latest vs code extension template (1.44)

## 1.0.42
 - Github issue #109 - "NavigatePane" type changed to the "NavigatePage" in the page wizard

 Thank you rvanbekkum for reporting the issue

## 1.0.41
 - New "Sort variables" code action

## 1.0.40
 - Added "Parent" property to the full syntax tree nodes
 - Double click on an entry in the list view of AL Object Browser opens object definition

## 1.0.39
 - New api function to parse source code and return full syntax tree

## 1.0.38
 - Github issue #102 - Symbols parser does not recognize multiple variables declared in the same line

 Thank you DavidFeldhoff for reporting the issue #102

## 1.0.37
 - Code actions functionality refactoring
 - Sort procedures, properties and report columns actions can now be run on document save

 Thank you rvanbekkum for the idea of running actions on document save

## 1.0.36
 - "Create Interface" code action availabe from codeunit object
 - Interface wizard

## 1.0.35
 - Github issue #97 - "Show all Project Symbols" doesn't list Page Customization and Profiles
 - Github enhancement #93 - New code action - sort object properties
 - Github enhancement #95 - New code action - sort report columns

 Thank you DavidFeldhoff, rvanbekkum and pri-kise for reporting your issues/enhancements 

## 1.0.34
 - Github issue #98 - AL Object Browser crashes with older app files

## 1.0.33
 - Interfaces support added to the symbols windows
 - Codeunit wizard with interface support

## 1.0.32
 - Github enhancement #89 - Display code analyzer rules from more than one analyzer

 Thank you rvanbekkum for the idea

## 1.0.31
 - "Sort procedures" command and code action by rvanbekkum

 Thank you rvanbekkum for your pull request

## 1.0.30
 - Github issue #82 - Add multiple fields in the order in which they have been selected

 Thank you Erik Hougaard for the idea

## 1.0.29
 - Github issue #70 - "Show Code Analyzer Rules" - Sorting
 - Github issue #71 - "Show Code Analyzer Rules" - Filtering

 Thank you rvanbekkum for reporting these issues

## 1.0.28
 - "Expand/Collapse child nodes" command in Symbols Tree View and AL Object Browser by rvanbekkum 
 - Github issue #75 - Symbols tree view scrollbars missing in AL Object Browser
 - Github issue #77 - Copy from Object Explorer 

 Thank you rvanbekkum for your pull request and issue reports and GreatScott000 for request #77
 
## 1.0.27
 - New functionality added by Stefano Primo:
   - code actions fixing CodeCop diagnostics 
     - AA0008 (add parentheses),
     - AA0137 (remove variable),
     - AA0139 (add CopyStr)
   - command to add missing application areas

   Thank you Stefano Primo for your pull request

## 1.0.26
 - Github issue #74 - Missing global variables in the symbols tree

## 1.0.25
 - Symbols service added to the extension API

## 1.0.24
 - Code analyzers rules viewer added after Dmitry Katson request
 - Html/js table control bugfixes

 Thank you Dmitry for your request

## 1.0.23
 - Syntax tree visualizer added. It shows current document syntax tree that can help developers to create their own al code analyzers. 
 - Support for Area and UserControl elements added to "Add multiple fields" code action on pages

## 1.0.22
 - Github issue #60 - Enable Object Navigation via Symbols Tree
 - Github issue #61 - Open documents on multiple tabs from "Go to definition" in "AL Object Browser". To enable this functionality please change "alOutline.openDefinitionInNewTab" setting to true
 - Github issue #62 - Event publishers support added to the xml documentation
 - XML documentation is enabled by default now, to turn it off change "alOutline.docCommentsType" setting to "none"
 - obsolete "enableFeaturePreview" setting removed

Thank you pri-kise, kevindstanley1988 and stefanoPrimo for reporting your issues and suggestions

## 1.0.21
 - Github issue #56 fixed - quick search in symbols tree ignores double quotes now

 Thank you pri-kise for reporting issue #56

## 1.0.20
 - Github issue #32 - Multi root workspace support added. Modified areas:
   - AL file wizards
   - "Show project symbols" commands 
 - Github issue #38 - Allow to run file wizards directly from the Command Palette

  Thank you pri-kise for posting additional information about isse #38

## 1.0.19
 - "Table wizard" added to the list of available "new al object" wizards

## 1.0.18
 - Github issue #53 - Alternative symbols path
 - Github issue #50 - Project Symbols and Document Symbols Tree do not show field nos.
 - If "Show Definition in the Symbols Tree" command cannot find definition for symbol at the current cursor position, symbols tree for the active document is opened

  Thank you YannRobertCargo and rvanbekkum for reporing these issues

## 1.0.17
 - Github issue #51 fixed - "Add multiple fields" code action broken after last update

## 1.0.16
 - Github issue #46 - Filter option for Symbols Browser/Outline
 - New Symbols Tree View showing document symbols in a WebView document. It can be opened from
   - AL Symbols Browser from object symbol context menu by selecting "Open symbol in new tab" option
   - "Show Document Symbols Tree" command from Command Palette when there is opened and acive text editor with AL file
   - Editor context menu by selecting "Show Definition in the Symbols Tree" menu item

 Thank you rvanbekkum for these 2 ideas

## 1.0.15
 - Xml documentation comments support added (but turned off by default)

## 1.0.14
 - "Add multiple field elements" and "Add multiple field attributes" code actions added to xml ports
 - Small changes to detecting language server for Microsoft AL Extension from Nav 2018 (work in progress)

## 1.0.13
 - Github issue #42 - Filtering in "Action Images" window
 - Keyboard navigation and context menu added to the "Action Images" window. Context menu contains these 3 actions:
   - Copy name - copies image name to the clipboard
   - Copy as action - generates page action element and copies it to the clipboard
   - Copy as promoted action - generates promoted page action element and copies it to the clipboard
   
   Enter key pressed when image list has focus, copies image name to the clipboard.

 Thank you rvanbekkum for reporting issue #42

## 1.0.12
 - The same filters ui for both list and tree views in symbol/object browsers
 - Better keyboard navigation and row selection in list view of symbols browser

## 1.0.11
 - Github issue #38 - Finish button on the object wizard not always works for some users - A few changes/fixes to improve wizards UI:
    - Added missing css style for disabled buttons
    - Finish button is always active (pressing Next and then Back was disabling it on the first page of all wizards)

## 1.0.10
 - Github issue #34 - Support for table extension, page extensions and enum extensions added to 'Go to definition' function in the symbol browsers
 - Github issue #37 - "Add multiple fields" code action can now also be invoked from fields to insert new elements below it. 
 - Different colors for obsolete and disabled fields icons in symbol browsers and al code outline panel

Thank you BrianThorChristensen for reporting issue #34, @sbineshji for reporting issue #37

## 1.0.9
 - CardPart page type added to the page wizard
 - Misspelled word in object wizard selection fixed

 Thank you @sbineshji for asking for CardParts support 

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