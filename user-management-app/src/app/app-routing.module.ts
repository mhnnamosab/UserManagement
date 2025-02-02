import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AuthGuard } from './core/guards/auth-guard.guard';
import { RoleGuard } from './core/guards/role.guard';
import { NotAuthorizedComponent } from './presentation/pages/not-authorized-component/not-authorized-component.component';
import { Roles } from './core/enums/roles.enum';
import { LoginComponent } from './presentation/pages/login/login.component';
import { MainLayoutComponent } from './presentation/layouts/main-layout/main-layout/main-layout.component';
const routes: Routes = [
  {
    path: '',
    component: MainLayoutComponent,
    children: [
      {
        path: 'users',
        loadChildren: () =>
          import('./features/users/users.module').then((m) => m.UsersModule),
        canActivate: [AuthGuard,RoleGuard],
        data: {
          roles: [Roles.ADMIN],
        },
      },
      { path: '', redirectTo: '/users', pathMatch: 'full' },
    ],
  },
  { path: 'login',  component : LoginComponent },
  { path: 'not-authorized', component: NotAuthorizedComponent }, // Not authorized page
  { path: '**', redirectTo: 'users' },

];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule { }
