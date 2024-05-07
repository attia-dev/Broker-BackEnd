import { Component, Injector, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { Observable, Subscription } from 'rxjs';
import { AppAuthService } from '@shared/auth/app-auth.service';
import { AppComponentBase } from '../../shared/app-component-base';
import { ChangePasswordDto, ResetPasswordDto, UserServiceProxy } from '@shared/service-proxies/service-proxies';
import { finalize } from 'rxjs/operators';
import { first } from 'rxjs/operators';
import { Router, ActivatedRoute } from '@angular/router';

enum ErrorStates {
    NotSubmitted,
    HasError,
    NoError,
}

@Component({
    selector: 'app-ResetPassword',
    templateUrl: './ResetPassword.component.html',
    styleUrls: ['./ResetPassword.component.scss'],
})
export class ResetPasswordComponent extends AppComponentBase implements OnInit {
    forgotPasswordForm: FormGroup;
    errorState: ErrorStates = ErrorStates.NotSubmitted;
    errorStates = ErrorStates;
    isLoading$: Observable<boolean>;
    ResetPasswordDto = new ResetPasswordDto();
    email: string;
    restCode: any;
    conPass: any;
    showError: boolean = false;
    constructor(injector: Injector, private fb: FormBuilder, private route: ActivatedRoute, private userService: UserServiceProxy, private authService: AppAuthService, private router: Router) {

        super(injector);

        this.ResetPasswordDto = new ResetPasswordDto();
        this.email = this.route.snapshot.paramMap.get('email');
        this.restCode = this.route.snapshot.paramMap.get('restCode');
        this.forgotPasswordForm = this.fb.group({
            newPassword: [this.ResetPasswordDto.newPassword, [Validators.required]],
            //    confirmNewPassword: [this.changePasswordDto., [Validators.required]],
        })
    }

    ngOnInit(): void {
        this.initForm();
    }

    get f() {
        return this.forgotPasswordForm.controls;
    }

    initForm() {
        this.forgotPasswordForm = this.fb.group({
            newPass: [this.ResetPasswordDto.newPassword, [Validators.required]],
            //    confirmNewPassword: [this.changePasswordDto., [Validators.required]],
        })
    }

    submit() {
       
        if (this.conPass != this.ResetPasswordDto.newPassword) {
            this.showError = true;
        }
        else {
            debugger;
            this.showError = false;
            this.ResetPasswordDto.usernameOrEmailAddress = this.email;
            this.ResetPasswordDto.resetCode = this.restCode;
            this.ResetPasswordDto.userId = 0;
            this.ResetPasswordDto.adminPassword = "123qwe";
            //this.userService.resetPasswordForForget(this.ResetPasswordDto);

            this.userService.resetPasswordForForget(this.ResetPasswordDto)
                .pipe(
                    finalize(() => {
                    })
                )
                .subscribe((data) => {
                    data == true ? this.notify.info(this.l('Common.Message.ActionSuccess')) : this.notify.error(this.l('Common.Message.DataIsNotValid'));
                    setTimeout(() => {
                       // this.router.navigate(["/account/login"]);
                    }, 1000);
                })


        }

    }
}
