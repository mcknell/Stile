var Stile;
(function (Stile) {
    (function (Prototypes) {
        (function (Specifications) {
            ///<reference path='..\..\..\Patterns\Structural\FluentInterface\hides.ts' />
            ///<reference path='Lifecycle\lifecycle.ts' />
            ///<reference path='..\specify.ts' />
            ///<reference path='..\SemanticModel\source.ts' />
            (function (Builders) {
                var ProcedureBuilder = (function () {
                    function ProcedureBuilder(source) {
                        this.source = source;
                        this.xray = this;
                    }
                    return ProcedureBuilder;
                })();
            })(Specifications.Builders || (Specifications.Builders = {}));
            var Builders = Specifications.Builders;
        })(Prototypes.Specifications || (Prototypes.Specifications = {}));
        var Specifications = Prototypes.Specifications;
    })(Stile.Prototypes || (Stile.Prototypes = {}));
    var Prototypes = Stile.Prototypes;
})(Stile || (Stile = {}));
//@ sourceMappingURL=procedureBuilder.js.map
