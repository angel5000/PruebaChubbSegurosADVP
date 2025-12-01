import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { FormControl } from '@angular/forms';
import { MatDrawerMode } from '@angular/material/sidenav';
import { Navigationitem } from './commons/sidenav/itemsidenav/navigations';
@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit {
  title = ""
  showSidenav = true;
  NombreUsuario: any;
  mode = new FormControl('push' as MatDrawerMode);
  items: Navigationitem[] = [
    { icons: 'bi bi-house fs-4', route: '/bienvenido', label: 'Principal' },
    { icons: 'bi bi-heart-pulse fs-4', route: '/seguros', label: 'Control de Seguros' },
    { icons: 'bi bi-person-arms-up fs-4', route: '/asegurados', label: 'Control de Asegurados' },
  ];
  filteredItems: Navigationitem[] = [];
  constructor(private router: Router) {
  }

  ngOnInit(): void {
  }

  Perfil() {
    this.router.navigate(["/perfil"]);
  }


}