import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SidenavComponent } from './sidenav.component';
import { SharedModule } from '../../shared/shared.module';
import { RouterModule } from '@angular/router';

@NgModule({
  declarations: [SidenavComponent],
  imports: [
    CommonModule,
    SharedModule,
    RouterModule,
  ],
  exports: [SidenavComponent]
})
export class SidenavModule {
}
