///<reference path='..\..\SemanticModel\source.ts' />

module Stile.Prototypes.Specifications.Builders.Lifecyle {
    
    export interface ILifeycleStage { }

    export interface IHasSource<TSubject> extends ILifeycleStage {
        source: SemanticModel.ISource<TSubject>;
    }

}
