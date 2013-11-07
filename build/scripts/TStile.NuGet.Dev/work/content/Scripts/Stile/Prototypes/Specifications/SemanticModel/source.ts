///<reference path='..\specify.ts' />

module Stile.Prototypes.Specifications.SemanticModel {
    export interface ISource<TSubject>{
        get(): TSubject;
    }

    class Source<TSubject> implements ISource<TSubject>{
        constructor(public subject: TSubject) { }

        public get(): TSubject {
            return this.subject;
        }
    }
}