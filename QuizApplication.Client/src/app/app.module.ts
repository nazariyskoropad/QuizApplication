import { BrowserModule } from '@angular/platform-browser';
import { NgModule, APP_INITIALIZER } from '@angular/core';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { HomeComponent } from './components/home/home.component';
import { LoginComponent } from './components/login/login.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { appInitializer } from './helpers/app.initializar';
import { AuthenticationService } from './services/authentication.service';
import { TestListComponent } from './components/test-list/test-list.component';
import { TestInfoComponent } from './components/test-info/test-info.component';
import { TestAddComponent } from './components/test-add/test-add.component';
import { TestEditComponent } from './components/test-edit/test-edit.component';

@NgModule({
  declarations: [
    AppComponent,
    HomeComponent,
    LoginComponent,
    TestListComponent,
    TestInfoComponent,
    TestAddComponent,
    TestEditComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    FormsModule,
    HttpClientModule,
    ReactiveFormsModule
  ],
  providers: [        
    { provide: APP_INITIALIZER, useFactory: appInitializer, multi: true, deps: [AuthenticationService] },
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
