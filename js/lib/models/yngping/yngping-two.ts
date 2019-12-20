
// Based on YngPing Specification 2.0

const YP_CONSONANTS = [
    'b', 'p', 'm', 'd', 't', 'n', 'l', 'z', 'c', 's', 'g', 'k', 'ng', 'h',
    'w', 'j', 'nj']

const YP_CONSONANTS_LONGEST_FIRST = YP_CONSONANTS.sort(s => -s.length);

const YP_VOWELS = ['a', 'o', 'e', 'oe', 'i', 'u', 'y']

const YP_CODAS = ['ng', 'h', 'k', '']

const YP_CODAS_LONGEST_FIRST = YP_CODAS.sort(s => -s.length);

const YP_TONES = [
    '55',    // 陰平
    '53',    // 陽平
    '33',    // 上聲
    '212',   // 陰去
    '242',   // 陽去
    '23',    // 陰入
    '5',     // 陽入
    '21',    // 半陰去
    '24'     // 半陽去
]

const VOWELS_WITH_DIACRITICS = {
    //     55   53   33   212  242   23   5    21   24
    'a': ['a', 'à', 'ā', 'ǎ', 'â', 'á', 'a', 'ǎ', 'á'],
    'o': ['o', 'ò', 'ō', 'ǒ', 'ô', 'ó', 'o', 'ǒ', 'ó'],
    'e': ['e', 'è', 'ē', 'ě', 'ê', 'é', 'e', 'ě', 'é'],
    'oe': ['ë', 'ë̀', 'ë̄', 'ë̌', 'ë̂', 'ë́', 'ë', 'ë̌', 'ë́'],
    'i': ['i', 'ì', 'ī', 'ǐ', 'î', 'í', 'i', 'ǐ', 'í'],
    'u': ['u', 'ù', 'ū', 'ǔ', 'û', 'ú', 'u', 'ǔ', 'ú'],
    'y': ['ü', 'ǜ', 'ǖ', 'ǚ', 'ü̂', 'ǘ', 'ü', 'ǚ', 'ǘ']
};

const HANDWRITTEN_VOWELS = getHandwrittenVowelsMappings();

const HANDWRITTEN_VOWELS_KEYS_LONGEST_FIRST =
    Object.keys(HANDWRITTEN_VOWELS).sort(s => -s.length);

interface HandwrittenVowelInfo {
    vowel: string,
    possibleTones: string[]
}
function getHandwrittenVowelsMappings(): { [x: string]: HandwrittenVowelInfo; } {
    let output = {};
    Object.keys(VOWELS_WITH_DIACRITICS)
        .forEach(v => {
            for (let i = 0; i < YP_TONES.length; ++i) {
                let h = VOWELS_WITH_DIACRITICS[v][i];
                if (!output[h]) {
                    output[h] = {
                        vowel: v,
                        possibleTones: []
                    };
                }
                output[h].possibleTones.push(YP_TONES[i]);
            }
        });
    return output;
}

export class YngPingTwoSyllable implements IYngPingTwoSyllable {

    private initial: string;
    private vowels: string[];
    private coda: string;
    private tone: string;

    constructor(initial: string, vowels: string[], coda: string, tone: string) {
        this.initial = initial;
        this.vowels = vowels;
        this.coda = coda;
        this.tone = tone;
    }

    public getInitial() {
        return this.initial;
    }

    getVowels = () => this.vowels;
    getCoda = () => this.coda;
    getTone = () => this.tone;


    public getTypingForm() {
        return this.initial + this.vowels.join('') + this.coda + this.tone;
    }
}

export function parseHandwriting(s: string, context?: ParsingContext)
    : YngPingTwoSyllable | ParseError {
    let remaining = s.normalize("NFC");
    let initial: string = '';
    let coda: string = '';
    let vowels = [];
    let tones: Set<string> = new Set();
    YP_TONES.forEach(t => {
        tones.add(t);
    });

    // Try rip off the initial consonant
    for (let i = 0; i < YP_CONSONANTS_LONGEST_FIRST.length; ++i) {
        if (remaining.toLowerCase().startsWith(
            YP_CONSONANTS_LONGEST_FIRST[i].normalize("NFC"))) {
            initial = YP_CONSONANTS_LONGEST_FIRST[i];
            remaining = remaining.substring(initial.length);
            break;
        }
    }

    // Try rip off the final consonant
    for (let i = 0; i < YP_CODAS_LONGEST_FIRST.length; ++i) {
        if (remaining.toLowerCase().endsWith(YP_CODAS_LONGEST_FIRST[i])) {
            coda = YP_CODAS_LONGEST_FIRST[i];
            remaining = remaining.substring(0, remaining.length - coda.length)
            break;
        }
    }


    let toneCarryingVowel = null;
    while (remaining.length > 0) {
        let hasAnyMatches = false;

        for (let i = 0; i < HANDWRITTEN_VOWELS_KEYS_LONGEST_FIRST.length; ++i) {
            if (remaining.startsWith(HANDWRITTEN_VOWELS_KEYS_LONGEST_FIRST[i])) {
                let matchHandwrittenVowel = HANDWRITTEN_VOWELS_KEYS_LONGEST_FIRST[i];
                remaining = remaining.substring(/* start= */ matchHandwrittenVowel.length);

                let vowelInfo = HANDWRITTEN_VOWELS[matchHandwrittenVowel];
                vowels.push(vowelInfo.vowel);
                if (VOWELS_WITH_DIACRITICS[vowelInfo.vowel][0] != matchHandwrittenVowel) {
                    if (toneCarryingVowel != null) {
                        return {
                            message: `Impossible tone: ${s}`
                        };
                    }
                    toneCarryingVowel = matchHandwrittenVowel;
                }

                hasAnyMatches = true;
                break;
            }
        }

        if (!hasAnyMatches) {
            return {
                message: `Unknown vowel: ${remaining}`
            } as ParseError;
        }
    }

    if (vowels.length == 0) {
        return { message: "No vowel!" } as ParseError;
    }

    if (!toneCarryingVowel) {
        tones = new Set(["55", "5"]);
    } else {
        tones = new Set(HANDWRITTEN_VOWELS[toneCarryingVowel].possibleTones);
    }

    // Now infer the tone
    let tone = null;
    let isMiddleOfPhrase = context?.isMiddleOfPhrase ?? false;
    if (tones.size == 1) {
        tone = [...tones][0];
    } else if (tones.size == 2) {
        if (tones.has("23") /* 陰入 */ && tones.has("24") /* 半陽去 */) {
            tone = (coda == 'h' || coda == 'k') ? '23' : '24';
        } else if (tones.has("55") /* 陰平 */ && tones.has("5") /* 陽入 */) {
            tone = (coda == 'h' || coda == 'k') ? '5' : '55';
        } else if (tones.has("212") /* 陰去 */ && tones.has("21") /* 半陰去 */) {
            tone = isMiddleOfPhrase ? '21' : '212';
        }
    } else {
        return {
            message: `Too many possible tonesl: ${[...tones]}`
        } as ParseError;
    }

    if (tone == null) {
        return {
            message: 'Impossible tone'
        }
    }

    return new YngPingTwoSyllable(initial, vowels, coda, tone);
}

export interface ParsingContext {
    isMiddleOfPhrase: boolean
}

export interface ParseError {
    message: string
}

interface IYngPingTwoSyllable {
    getInitial: () => string,
    getVowels: () => Array<string>,
    getCoda: () => string,
    getTone: () => string,
    getTypingForm: () => string,
}