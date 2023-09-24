import * as vscode from 'vscode';
import { ALConditionalCompilationSection } from "./alConditionalCompilationSection";
import { ALLineWordReader } from '../allanguage/alLineWordReader';

export class ALConditionalCompilationParser {    

    projectDirectives: string[] | undefined;

    constructor(directives: string[] | undefined) {
        this.projectDirectives = directives;
    }

    parseDocument(document: vscode.TextDocument): ALConditionalCompilationSection[] {
        let sections: ALConditionalCompilationSection[] = [];
        let active: ALConditionalCompilationSection | undefined = undefined;
        let mlComment: boolean = false;
        let wordReader = new ALLineWordReader();        
        let directives: string[] = [];

        if (this.projectDirectives) {        
            for (let i=0; i<this.projectDirectives.length; i++) {
                directives.push(this.projectDirectives[i]);
            }
        }

        //parse document and collect conditional compilation areas
        for (let i = 0; i < document.lineCount; i++) {
            wordReader.setLine(document.lineAt(i).text.trim());
            let word = wordReader.nextWord();
            if (word) {
                if (word === "#if") {
                    let newSection: ALConditionalCompilationSection = new ALConditionalCompilationSection(active, i);
                    newSection.enabled = this.parseCondition(wordReader, directives);
                    newSection.levelEnabled = newSection.enabled;
                    if (active) {
                        active.childSections.push(newSection);
                    } else {
                        sections.push(newSection);
                    }
                    active = newSection;
                } else if (word === "#elif") {
                    if (active) {
                        active.end = i;
                        let newSection: ALConditionalCompilationSection = new ALConditionalCompilationSection(active.parent, i);
                        newSection.enabled = (!active.levelEnabled) && (this.parseCondition(wordReader, directives));
                        newSection.levelEnabled = active.levelEnabled || newSection.enabled;
                        if (active.parent) {
                            active.parent.childSections.push(newSection);
                        } else {
                            sections.push(newSection);
                        }
                        active = newSection;
                    }
                } else if (word === "#else") {
                    if (active) {
                        active.end = i;
                        let newSection: ALConditionalCompilationSection = new ALConditionalCompilationSection(active.parent, i);
                        newSection.enabled = !active.levelEnabled;
                        newSection.levelEnabled = true;
                        if (active.parent) {
                            active.parent.childSections.push(newSection);
                        } else {
                            sections.push(newSection);
                        }
                        active = newSection;
                    }
                } else if (word === "#endif") {
                    if (active) {
                        active.end = i;
                        active = active.parent;
                    }
                } else if (word === "#define") {
                    
                    while (!wordReader.isEOL()) {
                        let defineWord = wordReader.nextWord();
                        if (defineWord)
                            directives.push(defineWord);
                    }

                } else if (word === "#undef") {
                    
                    while (!wordReader.isEOL()) {
                        let undefWord = wordReader.nextWord();
                        if (undefWord) {
                            let index = directives.indexOf(undefWord);
                            if (index >= 0)
                                directives.splice(index, 1);
                        }
                    }

                } else {
                    wordReader.readToEnd();
                }
            }
        }

        return sections;
    }

    private parseCondition(wordReader: ALLineWordReader, directives: string[]): boolean {
        let start = 0;
        let jscondition = "";

        while (!wordReader.isEOL()) {   
            let word = wordReader.nextWord();
            if (!word)
                break;

            if (word === 'and')
                jscondition += '&&';
            else if (word === 'or')
                jscondition += '||';
            else if (word === 'not')
                jscondition += '!';
            else if ((word === '(') || (word === ')'))
                jscondition += word;
            else if (directives.indexOf(word) >= 0)
                jscondition += 'true';
            else
                jscondition += 'false';
        }

        try
        {
            let val = eval(jscondition);
            return !!val;
        }
        catch (e) {
            return false;
        }
    }

}