using ASY.Hrefs.DAL.IRepository;
using ASY.Hrefs.Model.Models;
using AutoMapper;
using Grpc.Core;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;

namespace MicroServices
{
    public class HrefsService : Hrefs.HrefsBase
    {
        private IArticleRepository _articleRepository;
        private ILinkRepository _linkRepository;
        private IAccountRepository _accountRepository;
        private ICusLinkRepository _cusLinkRepository;
        private readonly ILogger<HrefsService> _logger;
        private IMapper _mapper;
        public HrefsService(ILogger<HrefsService> logger, IArticleRepository articleRepository, ILinkRepository linkRepository, IAccountRepository accountRepository, ICusLinkRepository cusLinkRepository)
        {
            _articleRepository = articleRepository;
            _linkRepository = linkRepository;
            _accountRepository = accountRepository;
            _cusLinkRepository = cusLinkRepository;
            _logger = logger;

            var configuration = Mapping.GetMapperConfiguration();
            _mapper = configuration.CreateMapper();
        }

        public override Task<LinkListResponse> GetAllLink(Empty request, ServerCallContext context)
        {
            var result = _linkRepository.GetAllLink();
            LinkListResponse response = new LinkListResponse();
            foreach(var item in result)
            {
                response.Items.Add(_mapper.Map<LinkProto>(item));
            }

            return Task.FromResult(response);
        }

        public override Task<LinkListResponse> ListLinkByCat(GlobalRequest request, ServerCallContext context)
        {
            var result = _linkRepository.ListLinkByCat(request.Catid);
            var response = new LinkListResponse();
            response.Items.AddRange(result.Select(p => _mapper.Map<LinkProto>(p)));
            return Task.FromResult(response);
        }

        public override Task<GlobalResponse> LinksVisitedCount(Empty request, ServerCallContext context)
        {
            var result = _linkRepository.LinksVisitedCount();
            return Task.FromResult(new GlobalResponse { Result = result });
        }

        public override Task<ArticleListResponse> ListArticleByPaging(GlobalRequest request, ServerCallContext context)
        {
            var result = _articleRepository.ListArticleByPaging(request.Size, request.Skip, request.Fields);
            var response = new ArticleListResponse();
            foreach(var item in result)
            {
                response.Items.Add(_mapper.Map<ArticleProto>(item));
            }

            return Task.FromResult(response);
        }

        public override Task<LinkCatListResponse> LinkCatList(Empty request, ServerCallContext context)
        {
            var result = _linkRepository.LinkCatList();
            LinkCatListResponse response = new LinkCatListResponse();
            foreach (var item in result)
            {
                response.Items.Add(_mapper.Map<LinkCatProto>(item));
            }

            return Task.FromResult(response);
        }

        public override Task<LinkCatProto> GetLinkCat(GlobalRequest request, ServerCallContext context)
        {
            var result = _linkRepository.GetLinkCat(request.Id);
            var response = _mapper.Map<LinkCatProto>(result);
            return Task.FromResult(response);
        }

        public override Task<ArticleProto> GetArticle(GlobalRequest request, ServerCallContext context)
        {
            var result = _articleRepository.GetArticle(request.Id, request.Fields);
            var response = _mapper.Map<ArticleProto>(result);
            return Task.FromResult(response);
        }

        public override Task<GlobalResponse> UpdatedLinkVisited(GlobalRequest request, ServerCallContext context)
        {
            var result = _linkRepository.UpdatedLinkVisited(request.Id);
            return Task.FromResult(new GlobalResponse { Result = result });
        }

        public override Task<LinkProto> GetLink(GlobalRequest request, ServerCallContext context)
        {
            var result = _linkRepository.GetLink(request.Id, request.Fields);
            var response = _mapper.Map<LinkProto>(result);
            return Task.FromResult(response);
        }

        public override Task<LinkPagerResponse> PagerLinkList(GlobalRequest request, ServerCallContext context)
        {
            var total = 0;
            var result = _linkRepository.PagerLinkList(request.Size, request.Offset, request.Catid, request.Title, request.Url, out total);
            var response = new LinkPagerResponse();
            response.Total = total;
            response.Items.AddRange(result.Select(p => _mapper.Map<LinkProto>(p)));
            return Task.FromResult(response);
        }

        public override Task<GlobalResponse> SaveLink(LinkProto request, ServerCallContext context)
        {
            var item = _mapper.Map<Link>(request);
            var result = _linkRepository.SaveLink(item);
            return Task.FromResult(new GlobalResponse { Result = result });
        }

        public override Task<GlobalResponse> DeleteLink(GlobalRequest request, ServerCallContext context)
        {
            var result = _linkRepository.DeleteLink(request.Id);
            return Task.FromResult(new GlobalResponse { Result = result });
        }

        public override Task<GlobalResponse> DeleteArticle(GlobalRequest request, ServerCallContext context)
        {
            var result = _articleRepository.DeleteArticle(request.Id);
            return Task.FromResult(new GlobalResponse { Result = result });
        }

        public override Task<GlobalResponse> UpdatedArticleVisited(GlobalRequest request, ServerCallContext context)
        {
            var result = _articleRepository.UpdatedArticleVisited(request.Id);
            return Task.FromResult(new GlobalResponse { Result = result });
        }

        public override Task<ArticlePagerResponse> PagerArticleList(GlobalRequest request, ServerCallContext context)
        {
            var total = 0;
            var result = _articleRepository.PagerArticleList(request.Size, request.Skip, request.Id, request.Title, out total, request.Fields);
            var response = new ArticlePagerResponse();
            response.Total = total;
            response.Items.AddRange(result.Select(p => _mapper.Map<ArticleProto>(p)));
            return Task.FromResult(response);
        }

        public override Task<GlobalResponse> SaveArticle(ArticleProto request, ServerCallContext context)
        {
            var item = _mapper.Map<Article>(request);
            var result = _articleRepository.SaveArticle(item);
            return Task.FromResult(new GlobalResponse { Result = result });
        }

        public override Task<GlobalResponse> DeleteAccount(GlobalRequest request, ServerCallContext context)
        {
            var result = _accountRepository.DeleteAccount(request.Id);
            return Task.FromResult(new GlobalResponse { Result = result });
        }

        public override Task<AccountPagerResponse> PagerAccountList(GlobalRequest request, ServerCallContext context)
        {
            var total = 0;
            var result = _accountRepository.PagerAccountList(request.Size, request.Skip, null, out total, request.Fields);
            var response = new AccountPagerResponse();
            response.Total = total;
            response.Items.AddRange(result.Select(p => _mapper.Map<AccountProto>(p)));
            return Task.FromResult(response);
        }

        public override Task<GlobalResponse> SaveAccount(AccountProto request, ServerCallContext context)
        {
            var item = _mapper.Map<Account>(request);
            var result = _accountRepository.SaveAccount(item);
            return Task.FromResult(new GlobalResponse { Result = result });
        }

        public override Task<AccountProto> GetAccount(GlobalRequest request, ServerCallContext context)
        {
            var result = _accountRepository.GetAccount(request.Id, request.Fields);
            var response = _mapper.Map<AccountProto>(result);
            return Task.FromResult(response);
        }

        public override Task<AccountProto> GetLogin(GlobalRequest request, ServerCallContext context)
        {
            var result = _accountRepository.GetLogin(request.Id, request.Password, request.Fields);
            var response = _mapper.Map<AccountProto>(result);
            return Task.FromResult(response);
        }

        public override Task<CusLinkProto> GetCusLink(GlobalRequest request, ServerCallContext context)
        {
            var result = _cusLinkRepository.GetCusLink(request.Id, request.Fields);
            var response = _mapper.Map<CusLinkProto>(result);
            return Task.FromResult(response);
        }

        public override Task<CusLinkPagerResponse> PagerCusLinkList(GlobalRequest request, ServerCallContext context)
        {
            var total = 0;
            var result = _cusLinkRepository.PagerCusLinkList(request.Size, request.Offset, request.Catid, request.Title, request.Url, out total);
            var response = new CusLinkPagerResponse();
            response.Total = total;
            response.Items.AddRange(result.Select(p => _mapper.Map<CusLinkProto>(p)));
            return Task.FromResult(response);
        }

        public override Task<GlobalResponse> SaveCusLink(CusLinkProto request, ServerCallContext context)
        {
            var item = _mapper.Map<CusLink>(request);
            var result = _cusLinkRepository.SaveCusLink(item);
            return Task.FromResult(new GlobalResponse { Result = result });
        }

        public override Task<GlobalResponse> DeleteCusLink(GlobalRequest request, ServerCallContext context)
        {
            var result = _cusLinkRepository.DeleteCusLink(request.Id);
            return Task.FromResult(new GlobalResponse { Result = result });
        }

        public override Task<TopCusLinkResponse> GetTopCusLink(GlobalRequest request, ServerCallContext context)
        {
            var result = _cusLinkRepository.GetTopCusLink(request.Size, request.Fields);
            var response = new TopCusLinkResponse();
            foreach (var item in result)
            {
                response.Items.Add(_mapper.Map<CusLinkProto>(item));
            }

            return Task.FromResult(response);
        }

        public override Task<CusLinkListResponse> ListCusLinkByPaging(GlobalRequest request, ServerCallContext context)
        {
            var result = _cusLinkRepository.ListCusLinkByPaging(request.Size, request.Skip, request.Fields);
            var response = new CusLinkListResponse();
            foreach (var item in result)
            {
                response.Items.Add(_mapper.Map<CusLinkProto>(item));
            }

            return Task.FromResult(response);
        }

        public override Task<GlobalResponse> UpdatedCusLinkVisited(GlobalRequest request, ServerCallContext context)
        {
            var result = _cusLinkRepository.UpdatedCusLinkVisited(request.Id);
            return Task.FromResult(new GlobalResponse { Result = result });
        }
    }
}