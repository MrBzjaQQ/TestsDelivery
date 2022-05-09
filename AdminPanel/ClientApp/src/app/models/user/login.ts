export interface LoginRequestModel {
    userName: string,
    password: string
}

export interface LoginSucceedResponseModel {
    userName: string,
    authenticationToken: string
}

export interface AuthenticationInfo {
    userName: string,
    authenticationToken: string
}