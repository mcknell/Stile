var Stile;
(function (Stile) {
    (function (Prototypes) {
        (function (Specifications) {
            ///<reference path='..\specify.ts' />
            (function (SemanticModel) {
                var Source = (function () {
                    function Source(subject) {
                        this.subject = subject;
                    }
                    Source.prototype.get = function () {
                        return this.subject;
                    };
                    return Source;
                })();
            })(Specifications.SemanticModel || (Specifications.SemanticModel = {}));
            var SemanticModel = Specifications.SemanticModel;
        })(Prototypes.Specifications || (Prototypes.Specifications = {}));
        var Specifications = Prototypes.Specifications;
    })(Stile.Prototypes || (Stile.Prototypes = {}));
    var Prototypes = Stile.Prototypes;
})(Stile || (Stile = {}));
//@ sourceMappingURL=source.js.map
