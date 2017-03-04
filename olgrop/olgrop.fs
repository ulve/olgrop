module MyWebApi.Program

open Suave
open Suave.Successful
open Suave.Operators
open Suave.Filters

type SlackRequest =
    {
        Token       : string
        TeamId      : string
        TeamDomain  : string
        ChannelId   : string
        ChannelName : string
        UserId      : string
        UserName    : string
        Command     : string
        Text        : string
        ResponseUrl : string
    }
    static member FromHttpContext (ctx : HttpContext) =
        let get key =
            match ctx.request.formData key with
            | Choice1Of2 x  -> x
            | _             -> ""
        {
            Token       = get "token"
            TeamId      = get "team_id"
            TeamDomain  = get "team_domain"
            ChannelId   = get "channel_id"
            ChannelName = get "channel_name"
            UserId      = get "user_id"
            UserName    = get "user_name"
            Command     = get "command"
            Text        = get "text"
            ResponseUrl = get "response_url"
        }

let janne (text: string) =
    "Janne är framtidens man"    
let janneHandler =
    fun (ctx : HttpContext) ->
    (SlackRequest.FromHttpContext ctx
    |> fun req ->
        req.Text
        |> janne
        |> OK) ctx

let app = POST >=> path "/janne" >=> janneHandler

[<EntryPoint>]
let main argv =

    let cfg = {defaultConfig with bindings = [HttpBinding.createSimple HTTP "192.168.1.14" 8080]}
    startWebServer cfg app
    0
