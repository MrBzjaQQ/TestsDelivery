export interface RegisterRequestModel {
    userName: string,
    password: string,
    passwordConfirm: string,
    email: string
}

export interface RegisterSucceedResponseModel {
    id: string,
    userName: string,
    email: string
}