import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { HomeComponent } from './components/home/home.component';
import { LoginComponent } from './components/login/login.component';
import { AuthGuard } from './guards/auth.guard';
import { TestListComponent } from './components/test-list/test-list.component';
import { TestInfoComponent } from './components/test-info/test-info.component';
import { TestAddComponent } from './components/test-add/test-add.component';
import { TestEditComponent } from './components/test-edit/test-edit.component';


const routes: Routes = [
  { path: '', component: HomeComponent, canActivate: [AuthGuard] },
  { path: 'login', component: LoginComponent },
  { path: 'admin/tests/add', component: TestAddComponent, canActivate: [AuthGuard]},
  { path: 'admin/tests/:id', component: TestInfoComponent, canActivate: [AuthGuard]},
  { path: 'admin/tests/:id/edit', component: TestEditComponent, canActivate: [AuthGuard]},
  { path: 'admin/tests', component: TestListComponent, canActivate: [AuthGuard]},



  { path: '**', redirectTo: '' }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
