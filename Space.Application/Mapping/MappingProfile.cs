using AutoMapper;
using Space.Application.DTOs.Comments;
using Space.Application.DTOs.Follows;
using Space.Application.DTOs.Likes;
using Space.Application.DTOs.Messages;
using Space.Application.DTOs.Notifications;
using Space.Application.DTOs.Posts;
using Space.Application.DTOs.Users;
using Space.Domain.Entities;
using Space.Domain.Identity;

namespace Space.Application.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Post, PostDto>()
                 .ForMember(dest => dest.User, opt => opt.MapFrom(src => src.User))
                 .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.UserName))
                 .ForMember(dest => dest.LikesCount, opt => opt.MapFrom(src => src.Likes.Count))
                 .ForMember(dest => dest.LikesUserNames, opt => opt.MapFrom(src => src.Likes.Select(l => l.User.UserName).ToList()))
                 .ForMember(dest => dest.LikesIds, opt => opt.MapFrom(src => src.Likes.Select(l => l.UserId).ToList()));

            CreateMap<CreatePostDto, Post>()
                .ForMember(dest => dest.ImagePath, opt => opt.Ignore())
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UserId, opt => opt.Ignore());

            CreateMap<UpdatePostDto, Post>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore());

            CreateMap<RegisterUserDto, ApplicationUser>()
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore());

            CreateMap<UpdateUserDto, ApplicationUser>()
           .ForMember(dest => dest.CreatedAt, opt => opt.Ignore());

            CreateMap<UpdateUserDto, ApplicationUser>()
            .ForMember(dest => dest.UserName, opt => opt.Condition(src => src.UserName != null))
            .ForMember(dest => dest.Email, opt => opt.Condition(src => src.Email != null))
            .ForMember(dest => dest.Bio, opt => opt.Condition(src => src.Bio != null))
            .ForMember(dest => dest.ProfileImagePath, opt => opt.Ignore());

            CreateMap<ApplicationUser, UserDto>();

            CreateMap<ApplicationUser, FollowUserDto>()
                .ForMember(dest => dest.ProfileImagePath, opt => opt.MapFrom(src => src.ProfileImagePath ?? string.Empty));

            CreateMap<Like, LikeDto>().ReverseMap();

            CreateMap<Comment, CommentDto>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.UserName))
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.User.Id))
                .ForMember(dest => dest.PostId, opt => opt.MapFrom(src => src.Post.Id))
                .ForMember(dest => dest.ProfileImgPath, opt => opt.MapFrom(src => src.User.ProfileImagePath));

            CreateMap<CommentDto, Comment>()
                .ForMember(dest => dest.User, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.Id, opt => opt.Ignore());

            CreateMap<CreateCommentDto, Comment>();
            CreateMap<CreateNotificationDto, Notification>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore());

            CreateMap<Notification, NotificationDto>()
                .ForMember(dest => dest.ActorUserName, opt => opt.MapFrom(src => src.Actor.UserName))
                .ForMember(dest => dest.ActorProfileImage, opt => opt.MapFrom(src => src.Actor.ProfileImagePath));

            CreateMap<Message, MessageDto>()
                .ForMember(dest => dest.SenderName, opt => opt.MapFrom(src => src.Sender.UserName))
                .ForMember(dest => dest.ReceiverName, opt => opt.MapFrom(src => src.Receiver.UserName))
                .ForMember(dest => dest.SenderImagePath, opt => opt.MapFrom(src => src.Receiver.ProfileImagePath))
                .ForMember(dest => dest.ReceiverImagePath, opt => opt.MapFrom(src => src.Receiver.ProfileImagePath));

            CreateMap<MessageDto, Message>()
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore());
            CreateMap<CreateMessageDto, Message>();
        }
    }
}