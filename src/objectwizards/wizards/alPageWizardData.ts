'use strict';

//import * as vscode from 'vscode';
import { ALTableBasedWizardData } from "./alTableBasedWizardData";
import { ALPageWizardFastTabData } from "./alPageWizardFastTabData";

export class ALPageWizardData extends ALTableBasedWizardData {
    pageType : string;
    fastTabs : string;
    usageCategory : string;
    caption : string;
    apiPublisher : string;
    apiGroup : string;
    apiVersion : string;
    entityName : string;
    entitySetName : string;

    fastTabsData : ALPageWizardFastTabData[];

    constructor() {
        super();
        this.pageType = "Card";
        this.fastTabs = "General";
        //this.appArea = vscode.workspace.getConfiguration('azALDevTools').get('defaultAppArea');
        //this.usageCategory = vscode.workspace.getConfiguration('azALDevTools').get('defaultListUsageCategory');
        this.applicationArea = "All";
        this.usageCategory = "";
        //api fields
        this.caption = "";
        this.apiPublisher = "publisherName";
        this.apiGroup = "apiGroup";
        this.apiVersion = "v1.0";
        this.entityName = "entityName";
        this.entitySetName = "entitySetName"
        //fast tabs
        this.fastTabsData = [];
    }

    isFastTabsPageType() : boolean {
        return ((this.pageType == "Card") || (this.pageType == "Document") || (this.pageType == "CardPart") ||
            (this.pageType == "ConfirmationDialog") || (this.pageType == "NavigatePage"));
    }

}