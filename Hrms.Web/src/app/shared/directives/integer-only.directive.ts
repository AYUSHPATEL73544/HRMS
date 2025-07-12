import { Directive, ElementRef, HostListener } from '@angular/core';

@Directive({
    selector: '[appIntegerOnly]'
})

export class IntegerOnlyDirective {
    regexStr = '^[0-9]*$';

    constructor(private el: ElementRef) { }

    @HostListener('keypress', ['$event']) onKeyPress(event: KeyboardEvent): boolean {
        return new RegExp(this.regexStr).test(event.key);
    }

    @HostListener('keyup', ['$event']) onKeyUp(event: KeyboardEvent): void {
        this.validateFields(event);
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
            const validtedValue = this.el.nativeElement.value.replace(/[^0-9]/g, '');
            if (validtedValue) {
                this.el.nativeElement.value = parseInt(validtedValue.toString().trim(), 0);
            } else {
                this.el.nativeElement.value = validtedValue.toString().trim();
            }
            this.el.nativeElement.dispatchEvent(new Event('input'));
        });
    }
}
