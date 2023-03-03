import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { ColorPipe } from './pipes/color.pipe';
import { YearPipe } from './pipes/year.pipe';
import { EnumPipe } from './pipes/enum.pipe';
import { ShortenPipe } from './pipes/shorten.pipe';

@NgModule({
    declarations: [
        ColorPipe,
        YearPipe,
        EnumPipe,
        ShortenPipe,
    ],
    imports: [
        CommonModule, 
    ],
    exports: [
        ColorPipe,
        YearPipe,
        EnumPipe,
        ShortenPipe,
    ]
  })

  export class SharedModule { }