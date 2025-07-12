import { Directive, Input } from '@angular/core';
import { NG_VALIDATORS, Validator, FormControl } from '@angular/forms';

@Directive({
    // tslint:disable-next-line: directive-selector
    selector: '[equalTo][ngModel]',
    providers: [{ provide: NG_VALIDATORS, useExisting: EqualToDirective, multi: true }]
})

export class EqualToDirective implements Validator {
    @Input() equalTo: string;

    validate(c: FormControl): { [key: string]: any } {
        const v = c.value;
        return this.equalTo !== null
            && this.equalTo !== undefined
            && v !== null
            && v !== undefined
            && this.equalTo !== v
            ? { equalTo: true }
            : null;
    }
}
