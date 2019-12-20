import "jasmine"
import { YngPingTwoSyllable, parseHandwriting, ParseError } from "@hokchewjs/lib/models/yngping/yngping-two";
import { ParsingContext } from "./yngping-two";

describe("YngPingTwoSyllable parsing test", function () {
    it("parses handwriting and converts to typing (end-to-end)", function () {
        expect(_p("güong")).toBe("gyong55");
        expect(_p("sēik")).toBe("seik33");
        expect(_p("zēing")).toBe("zeing33");
        expect(_p("dëü")).toBe("doey55");
        expect(_p("njuǒ")).toBe("njuo212");
        expect(_p("në̀üng")).toBe("noeyng53");
        expect(_p("hǚ")).toBe("hy212");
        expect(_p("hǚ", { isMiddleOfPhrase: true })).toBe("hy21");
        expect(_p("ling")).toBe("ling55");
        expect(_p("nang")).toBe("nang55");
        expect(_p("dîu")).toBe("diu242");
        expect(_p("loh")).toBe("loh5");
        expect(_p("sǒ", { isMiddleOfPhrase: true })).toBe("so21");
        expect(_p("wáh")).toBe("wah23");
        expect(_p("lǐng")).toBe("ling212");
        expect(_p("lǐng", { isMiddleOfPhrase: true })).toBe("ling21");
        expect(_p("nì")).toBe("ni53");
        //expect(_p("ö̂")).toBe("o242");
        expect(_p("bung")).toBe("bung55");
        expect(_p("ngang")).toBe("ngang55");
        expect(_p("iéh")).toBe("ieh23");
    });

    it("parses non-normalized strings", function() {
        expect(_p("zo\u030C")).toBe("zo212")
        expect(_p("z\u01D2")).toBe("zo212")
    })

    function _p(s: string, context?: ParsingContext): string {
        let result = parseHandwriting(s, context);
        if ((result as ParseError).message) {
            return (result as ParseError).message;
        } else {
            return (result as YngPingTwoSyllable).getTypingForm();
        }
    }

});
