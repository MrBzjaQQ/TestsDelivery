import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { HomeComponent } from './components/home-component/home.component';
import { LoginFormComponent } from './components/login-form/login-form.component';
import { RegisterFormComponent } from './components/register-form/register-form.component';
import { SidePanelComponent } from './components/side-panel/side-panel.component';
import { CandidatesListComponent } from './components/candidates-list/candidates-list.component'

import { ReactiveFormsModule } from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatButtonModule } from '@angular/material/button';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import {MatCardModule} from '@angular/material/card';
import { MatInputModule } from '@angular/material/input';
import { MatTableModule } from '@angular/material/table';
import { MatPaginatorModule } from '@angular/material/paginator';
import { AuthTokenInterceptor } from './interceptors/auth-token.interceptor';
import { ManageCandidateDialogComponent } from './components/manage-candidate-dialog/manage-candidate-dialog.component';


@NgModule({
  declarations: [
    AppComponent,
    HomeComponent,
    LoginFormComponent,
    RegisterFormComponent,
    SidePanelComponent,
    CandidatesListComponent,
    ManageCandidateDialogComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    BrowserAnimationsModule,
    ReactiveFormsModule,
    MatFormFieldModule,
    MatButtonModule,
    HttpClientModule,
    MatCardModule,
    MatInputModule,
    MatTableModule,
    MatPaginatorModule
  ],
  providers: [
    { provide: HTTP_INTERCEPTORS, useClass: AuthTokenInterceptor, multi: true }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
