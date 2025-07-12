import { Directive, ElementRef, HostListener } from '@angular/core';

@Directive({
    selector: '[appAlphabetWithSpaceOnly]'
})

export class AlphabetWithSpaceOnlyDirective {
    regexStr = '^[a-zA-Z ]*$';

    constructor(private el: ElementRef) { }

    @HostListener('keypress', ['$event']) onKeyPress(event: KeyboardEvent): boolean {
        return new RegExp(this.regexStr).test(event.key);
    }

    @HostListener('paste', ['$event']) blockPaste(event: KeyboardEvent): void {
        this.validateFields(event);
    }

    @HostListener('drop', ['$event']) blockDrop(event: KeyboardEvent): void {
        this.validateFields(event);
    }

    @HostListener('blur', ['$event']) blockBlur(event: KeyboardEvent): void {
        this.validateFields(event);
    }

    validateFields(event: any): void {
        setTimeout(() => {
            event.preventDefault();
            const validtedValue = this.el.nativeElement.value.replace(/[^a-zA-Z ]/g, '');
            this.el.nativeElement.value = validtedValue.toString().trim();
            this.el.nativeElement.dispatchEvent(new Event('input'));
        });
    }
}
