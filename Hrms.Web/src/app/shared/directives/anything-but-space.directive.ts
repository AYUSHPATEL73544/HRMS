import { Directive, ElementRef, HostListener } from '@angular/core';

@Directive({
    selector: '[appAnythingButWhiteSpace]'
})

export class AnythingButWhiteSpaceDirective {
    regexStr = '[^\ ]';

    constructor(private el: ElementRef) { }

    @HostListener('keypress', ['$event']) onKeyPress(event: KeyboardEvent): boolean {
        return new RegExp(this.regexStr).test(event.key);
    }

    @HostListener('paste', ['$event']) blockPaste(event: KeyboardEvent): void {
        event.preventDefault();
    }

    @HostListener('drop', ['$event']) blockDrop(event: KeyboardEvent): void {
        event.preventDefault();
    }
}
