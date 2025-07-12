import { Pipe, PipeTransform } from '@angular/core';

@Pipe({ name: 'naIfEmpty' })
export class NaIfEmpty implements PipeTransform {
    transform(value: any): string {
        return value ? value : 'N/A';
    }
}
