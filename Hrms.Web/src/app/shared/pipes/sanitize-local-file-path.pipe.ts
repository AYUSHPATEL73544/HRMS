import { Pipe, PipeTransform } from '@angular/core';

@Pipe({ name: 'sanitizeLocalFilePath' })
export class SanitizeLocalFilePath implements PipeTransform {
    transform(value: string): string {
        return value ? value.replace(/^.*[\\\/]/, '') : '';
    }
}
