export interface LoginRequestModel {
    userName: string,
    password: string
}

export interface LoginSucceedResponseModel {
    userName: string,
    accessToken: string
}

export interface AuthenticationInfo {
    userName: string,
    accessToken: string
}