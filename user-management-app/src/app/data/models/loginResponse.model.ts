import { User } from "../../core/domain/models/user.model"

export class LoginResponse{
    token !: string
    user  !: User
}