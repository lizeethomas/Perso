import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
    name: 'color'
})

export class ColorPipe implements PipeTransform {

    transform(text: string, color:string) : string {
        let tmp:string = ''
        tmp += '<span style="' + color + '">' + text + '</span';
        return tmp;
    }

}