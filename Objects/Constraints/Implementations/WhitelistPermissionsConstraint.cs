using izolabella.Discord.Objects.Constants.Enums;
using izolabella.Discord.Objects.Constraints.Interfaces;

namespace izolabella.Discord.Objects.Constraints.Implementations
{
    /// <summary>
    /// A derivation of <see cref="IIzolabellaCommandConstraint"/> regarding a whitelist of users that may invoke the parent command if they hold the correct permissions in a text channel.
    /// </summary>
    public class WhitelistPermissionsConstraint : IIzolabellaCommandConstraint
    {
        /// <summary>
        /// Initializes an instance of the <see cref="WhitelistPermissionsConstraint"/> class.
        /// </summary>
        public WhitelistPermissionsConstraint(bool ValidOnlyInGuilds, params GuildPermission[] Permissions)
        {
            this.ValidOnlyInGuilds = ValidOnlyInGuilds;
            this.Permissions = Permissions;
        }

        /// <summary>
        /// If true, the constraint will not allow the command to be invoked unless it was invoked in a guild.
        /// </summary>
        public bool ValidOnlyInGuilds { get; }

        /// <summary>
        /// All of the permissions a user must possess to invoke this command.
        /// </summary>
        public GuildPermission[] Permissions { get; }

        /// <inheritdoc/>
        public ConstraintTypes Type => ConstraintTypes.WhitelistPermissions;

        /// <inheritdoc/>
        public ulong? ConstrainToOneGuildOfThisId { get; set; }

        Task<bool> IIzolabellaCommandConstraint.CheckCommandValidityAsync(SocketSlashCommand CommandFired)
        {
            return CommandFired.User is SocketGuildUser SUser && CommandFired.Channel is SocketTextChannel SChannel
                ? Task.FromResult(this.Permissions.All(SUser.GuildPermissions.Has))
                : Task.FromResult(!this.ValidOnlyInGuilds);
        }
    }
}
