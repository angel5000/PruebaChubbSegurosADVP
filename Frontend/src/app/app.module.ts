import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { AppComponent } from './app.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { HttpClientModule,  } from '@angular/common/http';

import { MatSnackBarModule } from '@angular/material/snack-bar';
import { MatInputModule } from '@angular/material/input';
import { MatFormFieldModule } from "@angular/material/form-field";
import { AppRoutingModule } from './app-routing.module';
import { SharedModule } from './shared/shared.module';
import { ItemsidenavComponent } from './commons/sidenav/itemsidenav/itemsidenav.component';
import { ToastrModule } from 'ngx-toastr';

@NgModule({
  declarations: [AppComponent, ItemsidenavComponent],
  imports: [
    BrowserModule,
    AppRoutingModule,
    BrowserAnimationsModule,
    HttpClientModule,
    MatSnackBarModule,
    MatInputModule,
    MatFormFieldModule,
    BrowserAnimationsModule, 
    ToastrModule.forRoot(), 
    SharedModule
],
  
  bootstrap: [AppComponent]
})
export class AppModule { }