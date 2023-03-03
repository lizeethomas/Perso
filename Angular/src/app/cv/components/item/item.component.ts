import { Component, Input } from '@angular/core';
import { CVItem } from '../../models/CVItem';

@Component({
  selector: 'app-item',
  templateUrl: './item.component.html',
  styleUrls: ['./item.component.scss']
})
export class ItemComponent {

  @Input() item!:CVItem;

}
