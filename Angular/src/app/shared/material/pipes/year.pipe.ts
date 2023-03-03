import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
    name: 'year'
})

export class YearPipe implements PipeTransform {

    transform(text: string) : number {
        return Number(text.split("-")[0]);
    }

}