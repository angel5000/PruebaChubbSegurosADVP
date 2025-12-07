import { Component, OnInit } from '@angular/core';
import { NavigationEnd, Router } from '@angular/router';
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
    { icons: 'bi bi-currency-dollar fs-4', route: '/cobranzas', label: 'Cobranzas' },
  ];
  filteredItems: Navigationitem[] = [];
  constructor(private router: Router) {
  }

  ngOnInit(): void {
    const hiddenRoutes = ['/login'];
    this.router.events.subscribe((event) => {
      if (event instanceof NavigationEnd) {
        // Oculta el sidenav si la URL está en las rutas ocultas
        this.showSidenav = !hiddenRoutes.includes(this.router.url);
      }
    });
    setTimeout(() => {
      const user = (localStorage.getItem('usuario') ?? '').replace(/"/g, '');
      this.NombreUsuario =user;
    }, 0);
   
  }

  Perfil() {
    this.router.navigate(["/perfil"]);
  }
  Cerrarsesion() {
    this.router.navigate(['/login']);
    this.showSidenav=false
    localStorage.clear();
    }

}