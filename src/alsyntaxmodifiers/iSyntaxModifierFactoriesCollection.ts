import { SyntaxModifier } from "./syntaxModifier";

export interface ISyntaxModifierFactoriesCollection {
    [name: string]: () => SyntaxModifier;
}