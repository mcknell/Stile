///<reference path='..\..\..\Patterns\Structural\FluentInterface\hides.ts' />
///<reference path='Lifecycle\lifecycle.ts' />
///<reference path='..\specify.ts' />
///<reference path='..\SemanticModel\source.ts' />

module Stile.Prototypes.Specifications.Builders {
    
    export interface IProcedureBuilder<TSubject> extends Patterns.Structural.FluentInterface.IHides<IProcedureBuilderState<TSubject>> {
        source: SemanticModel.ISource<TSubject>;
    }
    
    export interface IProcedureBuilderState<TSubject> extends Lifecyle.IHasSource<TSubject> {
        source: SemanticModel.ISource<TSubject>;
    }

    class ProcedureBuilder<TSubject> implements IProcedureBuilder<TSubject>, IProcedureBuilderState<TSubject> {
        constructor(public source: SemanticModel.ISource<TSubject>) {
            this.xray = this;
        }

        public xray: IProcedureBuilderState<TSubject>;
    }
}