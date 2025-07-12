import { Pipe, PipeTransform } from "@angular/core";

@Pipe({
    name: 'tooltipList',
})

export class TooltipList implements PipeTransform{
    transform(values:string[]):string {
        let list: string = '';
        if(values.length === 0 || values.includes('')){
            return 'No records available.'
        }
        else{
        values.forEach(value => {
            list += '• ' + value + '\n';
       });
    }
           return list;
    }
}