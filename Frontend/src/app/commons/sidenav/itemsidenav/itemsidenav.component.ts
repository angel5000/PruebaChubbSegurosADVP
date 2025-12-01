import { Component, HostBinding, Input } from '@angular/core';
import { Router } from '@angular/router';
import { Navigationitem} from './navigations';

@Component({
  selector: 'app-itemsidenav',
  templateUrl: './itemsidenav.component.html',
  styleUrl: './itemsidenav.component.scss'
})
export class ItemsidenavComponent {
  @Input() item: Navigationitem | undefined;
  @Input() level: number | undefined; 
  @Input() rol: number | undefined;
  constructor(private router: Router) {}
 
  @HostBinding('class')
  get levelClass() {
    return `item-level-${this.level}`;
  }
  filteredItems: Navigationitem[] = [];
  ngOnInit(): void {
  }
 
  navigate() {
    if (this.item?.route) {
      this.router.navigate([this.item.route]);
    }
  }

}
