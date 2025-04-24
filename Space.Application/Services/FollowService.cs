using AutoMapper;
using Space.Application.DTOs.Follows;
using Space.Application.DTOs.Notifications;
using Space.Application.Interfaces.Repositories;
using Space.Application.Interfaces.Services;
using Space.Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace Space.Application.Services
{
    public class FollowService : IFollowService
    {
        private readonly IFollowRepository followRepository;
        private readonly IMapper mapper;
        private readonly IUserRepository userRepository;
        private readonly INotificationService notificationService;

        public FollowService(IFollowRepository followRepository, IMapper mapper, IUserRepository userRepository, INotificationService notificationService)
        {
            this.followRepository = followRepository;
            this.mapper = mapper;
            this.userRepository = userRepository;
            this.notificationService = notificationService;
        }

        public async Task<bool> FollowUserAsync(string followerId, string followeeId)
        {
            if (string.IsNullOrEmpty(followerId))
                throw new ValidationException("Follower ID is required.");

            if (string.IsNullOrEmpty(followeeId))
                throw new ValidationException("Followee ID is required.");

            var follower = await userRepository.FindByIdAsync(followerId);
            if (follower == null)
                throw new KeyNotFoundException("Follower not found.");

            var followee = await userRepository.FindByIdAsync(followeeId);
            if (followee == null)
                throw new KeyNotFoundException("Followee not found.");

            if (followerId == followeeId)
                throw new ValidationException("Cannot follow yourself.");

            var isFollowing = await followRepository.IsFollowingAsync(followerId, followeeId);
            if (isFollowing)
                throw new ValidationException("You are already following this user.");
            var follow = new Follow
            {
                FollowerId = followerId,
                FolloweeId = followeeId,
                CreatedAt = DateTime.UtcNow
            };
            var result = await followRepository.AddFollowAsync(follow);
            if (followeeId != followerId)
            {
                await notificationService.CreateNotificationAsync(
                    followerId,
                    new CreateNotificationDto
                    {
                        UserId = followeeId,
                        ActorId = followerId,
                        Type = "Follow",
                        EntityId = follow.Id,
                        Title = $"{follower.UserName} followed you!"
                    }
                );
            }
            return result;
        }

        public async Task<bool> UnfollowUserAsync(string followerId, string followeeId)
        {
            if (string.IsNullOrEmpty(followerId))
                throw new ValidationException("Follower ID is required.");

            if (string.IsNullOrEmpty(followeeId))
                throw new ValidationException("Followee ID is required.");

            var follower = await userRepository.FindByIdAsync(followerId);
            if (follower == null)
                throw new KeyNotFoundException("Follower not found.");

            var followee = await userRepository.FindByIdAsync(followeeId);
            if (followee == null)
                throw new KeyNotFoundException("Followee not found.");

            if (followerId == followeeId)
                throw new ValidationException("Cannot unfollow yourself.");

            var isFollowing = await followRepository.IsFollowingAsync(followerId, followeeId);
            if (!isFollowing)
                throw new ValidationException("You are not following this user.");

            var follow = new Follow
            {
                FollowerId = followerId,
                FolloweeId = followeeId
            };

            return await followRepository.RemoveFollowAsync(followerId, followeeId);
        }

        public async Task<IEnumerable<FollowUserDto>> GetFollowersAsync(string userId)
        {
            if (string.IsNullOrEmpty(userId))
                throw new ValidationException("User ID is required.");
            var user = await userRepository.FindByIdAsync(userId);
            if (user == null)
                throw new KeyNotFoundException("User not found.");
            var followers = await followRepository.GetFollowersAsync(userId);
            return mapper.Map<IEnumerable<FollowUserDto>>(followers);
        }

        public async Task<IEnumerable<FollowUserDto>> GetFollowingAsync(string userId)
        {
            if (string.IsNullOrEmpty(userId))
                throw new ValidationException("User ID is required.");
            var user = await userRepository.FindByIdAsync(userId);
            if (user == null)
                throw new KeyNotFoundException("User not found.");
            var following = await followRepository.GetFollowingAsync(userId);
            return mapper.Map<IEnumerable<FollowUserDto>>(following);
        }

        public async Task<bool> IsFollowingAsync(string followerId, string followeeId)
        {
            if (string.IsNullOrEmpty(followerId))
                throw new ValidationException("follower id is required to check if following");
            if (string.IsNullOrEmpty(followeeId))
                throw new ValidationException("followee id is required to check if following");

            return await followRepository.IsFollowingAsync(followerId, followeeId);
        }
    }
}