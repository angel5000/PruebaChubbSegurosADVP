import { Component, OnInit } from '@angular/core';
import { NavigationEnd, Router } from '@angular/router';
import { FormControl } from '@angular/forms';
import { MatDrawerMode } from '@angular/material/sidenav';
import { Navigationitem } from './commons/sidenav/itemsidenav/navigations';
import { AuthService } from './pages/Auth/Services/auth.service';
import { Subject } from 'rxjs';
@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit {
  showSidenav = true;
  NombreUsuario: any;
  mode = new FormControl('push' as MatDrawerMode);
  items: Navigationitem[] = [
    { icons: 'bi bi-house fs-4', route: '/bienvenido', label: 'Principal' },
    { icons: 'bi bi-heart-pulse fs-4', route: '/seguros', label: 'Control de Seguros', requiredRole: [100, 101] },
    { icons: 'bi bi-person-arms-up fs-4', route: '/asegurados', label: 'Control de Asegurados', requiredRole: [100, 101] },
    { icons: 'bi bi-currency-dollar fs-4', route: '/cobranzas', label: 'Cobranzas', requiredRole: [101] },
  ];
  filteredItems: Navigationitem[] = [];
  constructor(private router: Router, private authService: AuthService) {
  }
  private destroy$ = new Subject<void>();
  ngOnInit(): void {
    const hiddenRoutes = ['/login', '/notfound'];
    this.router.events.subscribe((event) => {
      if (event instanceof NavigationEnd) {
        this.showSidenav = !hiddenRoutes.includes(this.router.url);
      }
    });

    setTimeout(() => {
      const user = (localStorage.getItem('usuario') ?? '').replace(/"/g, '');
      this.NombreUsuario = user;
    }, 0);

    const userRole = this.authService.getUserRole() ?? 0;
    this.filteredItems = this.items.filter(item =>
      item.requiredRole ? item.requiredRole.includes(userRole) : true
    );

  }
  ngOnDestroy() {
    this.destroy$.next();
    this.destroy$.complete();
  }

  Perfil() {
    this.router.navigate(["/perfil"]);
  }

  Cerrarsesion() {
    this.showSidenav = false
    this.router.navigate(['/login'], { replaceUrl: true });
    setTimeout(() => {
      localStorage.clear();
    }, 0);
  }

}