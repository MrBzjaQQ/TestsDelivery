import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { ReactiveFormsModule } from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatButtonModule } from '@angular/material/button';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { MatCardModule } from '@angular/material/card';
import { MatInputModule } from '@angular/material/input';
import { MatTableModule } from '@angular/material/table';
import { MatPaginatorModule } from '@angular/material/paginator';
import { MatDialogModule, MAT_DIALOG_DEFAULT_OPTIONS } from '@angular/material/dialog';
import { MatDividerModule } from '@angular/material/divider';
import { MatListModule } from '@angular/material/list';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatRadioModule } from '@angular/material/radio';
import { MatMenuModule } from '@angular/material/menu';
import { MatTreeModule } from '@angular/material/tree';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { HomeComponent } from './components/home-component/home.component';
import { LoginFormComponent } from './components/login-form/login-form.component';
import { RegisterFormComponent } from './components/register-form/register-form.component';
import { SidePanelComponent } from './components/side-panel/side-panel.component';
import { CandidatesListComponent } from './components/candidates-list/candidates-list.component'
import { AuthTokenInterceptor } from './interceptors/auth-token.interceptor';
import { ManageCandidateDialogComponent } from './components/manage-candidate-dialog/manage-candidate-dialog.component';
import { SubjectsListComponent } from './components/subjects-list/subjects-list.component';
import { ManageSubjectDialogComponent } from './components/manage-subject-dialog/manage-subject-dialog.component';
import { TestsListComponent } from './components/tests-list/tests-list.component';
import { ScheduledTestsListComponent } from './components/scheduled-tests-list/scheduled-tests-list.component';
import { ManageQuestionsComponent } from './components/manage-questions/manage-questions.component';
import { ManageEssayComponent } from './components/manage-essay/manage-essay.component';
import { ManageSingleChoiceComponent } from './components/manage-single-choice/manage-single-choice.component';
import { ManageMultipleChoiceComponent } from './components/manage-multiple-choice/manage-multiple-choice.component';
import { SearchBarComponent } from './components/search-bar/search-bar.component';
import { ManageTestComponent } from './components/manage-test/manage-test.component';


@NgModule({
  declarations: [
    AppComponent,
    HomeComponent,
    LoginFormComponent,
    RegisterFormComponent,
    SidePanelComponent,
    CandidatesListComponent,
    ManageCandidateDialogComponent,
    SubjectsListComponent,
    ManageSubjectDialogComponent,
    TestsListComponent,
    ScheduledTestsListComponent,
    ManageQuestionsComponent,
    ManageEssayComponent,
    ManageSingleChoiceComponent,
    ManageMultipleChoiceComponent,
    SearchBarComponent,
    ManageTestComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    BrowserAnimationsModule,
    ReactiveFormsModule,
    HttpClientModule,
    
    MatFormFieldModule,
    MatButtonModule,
    MatCardModule,
    MatInputModule,
    MatTableModule,
    MatPaginatorModule,
    MatDialogModule,
    MatDividerModule,
    MatListModule,
    MatCheckboxModule,
    MatRadioModule,
    MatMenuModule,
    MatTreeModule
  ],
  providers: [
    { provide: HTTP_INTERCEPTORS, useClass: AuthTokenInterceptor, multi: true },
    { provide: MAT_DIALOG_DEFAULT_OPTIONS, useValue: { hasBackdrop: true } }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
