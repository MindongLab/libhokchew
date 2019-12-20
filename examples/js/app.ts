import { YngPingTwoSyllable, ParseError, parseHandwriting } from "@hokchewjs/lib/models/yngping/yngping-two";

let txtInput: HTMLTextAreaElement = document.querySelector("#txtInput");
let txtOutput: HTMLTextAreaElement = document.querySelector("#txtOutput");

txtInput.oninput = () => {
    let txt = txtInput.value;
    txtOutput.value = convert(txt);
}

function convert(s: string): string {
    return tokenize(s).map(t => {
        let tryParse = parseHandwriting(t);
        if ((tryParse as ParseError).message) {
            return t;
        } else {
            return (tryParse as YngPingTwoSyllable).getTypingForm()
        }
    }).join('');
}

function tokenize(s: string): string[] {
    let output = [];
    let currentToken = '';
    let currentType: TokenType = null;

    function commit() {
        if (currentType == null) return;
        output.push(currentToken);
        currentToken = '';
        currentType = null;
    }
    for (let i = 0; i < s.length; ++i) {
        let newType = getTokenType(s[i]);
        if (newType != currentType) {
            commit();
            currentType = newType;
            currentToken += s[i];
            if (currentType == TokenType.SINGLE_SYMBOL) {
                // commit immediately
                commit();
            }
        } else {
            currentToken += s[i];
        }
    }
    commit();
    return output;
}

function getTokenType(c: string) {
    if (c == ' ' || c == '\t' || c == '\r' || c=='\n') {
        return TokenType.WHITESPACES;
    }
    else if (SINGLE_SYMBOLS.has(c)) {
        return TokenType.SINGLE_SYMBOL;
    }
    return TokenType.OTHER;
}


const SINGLE_SYMBOLS = new Set([
    '`',
    '"',
    "'",
    "\\",
    "/",
    ".",
    ",",
    '-',
    '_',
    '—',
    '，',
    '。'
])
enum TokenType {
    WHITESPACES,
    SINGLE_SYMBOL, // `",._-/\
    OTHER
}