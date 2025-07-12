import { Directive, Input, OnChanges, SimpleChanges } from "@angular/core";
import { FormControl, NG_VALIDATORS, Validator } from "@angular/forms";

@Directive({
    selector: '[greaterTo][ngModel]',
    providers: [{ provide: NG_VALIDATORS, useExisting: GreaterToDirective, multi: true }]
})

export class GreaterToDirective implements Validator, OnChanges {

    @Input() greaterTo: string;

    onChange: () => void;

    ngOnChanges(changes: SimpleChanges): void {
        if (changes['greaterTo'] && this.onChange) {
            this.onChange();
        }
    }

    registerOnValidatorChange(fn: () => void): void {
        this.onChange = fn;
    }

    validate(c: FormControl): { [Key: string]: any } {
        const v = c.value;
        return (this.greaterTo === null
            || this.greaterTo === undefined
            || String(v) < String(this.greaterTo))
            ? { greaterTo: true }
            : null;
    }
}