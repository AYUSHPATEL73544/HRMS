import { Directive, Input } from '@angular/core';
import { NG_VALIDATORS, Validator, FormControl } from '@angular/forms';

@Directive({
    // tslint:disable-next-line: directive-selector
    selector: '[maxNumber][ngModel]',
    providers: [{ provide: NG_VALIDATORS, useExisting: MaxNumberDirective, multi: true }]
})

export class MaxNumberDirective implements Validator {
    @Input() maxNumber: number;

    validate(c: FormControl): { [key: string]: any } {
        const v = c.value;
        return (this.maxNumber === null
            || this.maxNumber === undefined
            || Number(v) > Number(this.maxNumber))
            ? { maxNumber: true }
            : null;
    }
}
