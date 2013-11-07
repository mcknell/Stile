var Stile;
(function (Stile) {
    (function (Prototypes) {
        ///<reference path=".\Builders\procedureBuilder.ts" />
        (function (Specifications) {
            var Specify = (function () {
                function Specify() {
                }
                Specify.prototype.ForAny = function () {
                    return null;
                };
                return Specify;
            })();
            Specifications.Specify = Specify;
        })(Prototypes.Specifications || (Prototypes.Specifications = {}));
        var Specifications = Prototypes.Specifications;
    })(Stile.Prototypes || (Stile.Prototypes = {}));
    var Prototypes = Stile.Prototypes;
})(Stile || (Stile = {}));
//@ sourceMappingURL=specify.js.map
